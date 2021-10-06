using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using BioTower.UI;
using System;

namespace BioTower
{
public enum PlacementState
{
    NONE,
    PLACING,
}

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private PlacementState placementState;
    [SerializeField] private LayerMask socketLayerMask;
    private StructureType structureToPlace;
    [SerializeField] private Vector3 placementOffset;



    [Header("Structure prefabs")]
    [SerializeField] private GameObject abaTowerPrefab;
    [SerializeField] private GameObject ppc2TowerPrefab;
    [SerializeField] private GameObject chloroplastTowerPrefab;
    [SerializeField] private GameObject mitoTowerPrefab;


    private void Awake()
    {
        structureToPlace = StructureType.NONE;
    }

    public GameObject CreateStructure(StructureType structureType)
    {
        GameObject tower = null;
        switch(structureType)
        {
            case StructureType.ABA_TOWER:
                tower = Instantiate(abaTowerPrefab);
                break;
            case StructureType.PPC2_TOWER:
                tower = Instantiate(ppc2TowerPrefab);
                break;
            case StructureType.CHLOROPLAST:
                tower = Instantiate(chloroplastTowerPrefab);
                break;
            case StructureType.MITOCHONDRIA:
                tower = Instantiate(mitoTowerPrefab);
                break;
        }

        var structure = tower.GetComponent<Structure>();
        return tower;
    }

    private void OnPressTowerButton(StructureType structureType)
    {
        SetPlacingState(structureType);   
    }

    private void OnTouchBegan(Vector3 screenPos)
    {
        if (!InputController.canSpawnTowers)
            return;

        if (IsPlacingState())
        {
            if (GameManager.Instance.econManager.CanBuyTower(structureToPlace))
            {
                PlaceTower(screenPos);
                //TapStructure(screenPos);
            }
        }
    }

    private void PlaceTower(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, socketLayerMask);
        //Debug.Log(hitInfo.collider.gameObject.name);
        if (hitInfo.collider != null)
        {
            var socket = hitInfo.collider.transform.parent.GetComponent<StructureSocket>();
            if (!socket.HasStructure() && socket.CanAcceptStructure(structureToPlace))
            {
                socket.SetHasStructure(true);
                var tower = CreateStructure(structureToPlace);
                tower.transform.position = hitInfo.collider.transform.position + placementOffset;
                var structure = tower.GetComponent<Structure>();
                structure.Init(socket);
                GameManager.Instance.econManager.BuyTower(structureToPlace);
                SetNoneState();
            }
        }
    }

   
    private void OnStartPlacementState(StructureType structureType)
    {
        var tower = CreateStructure(structureType);
    }


    public void SetNoneState() 
    { 
        placementState = PlacementState.NONE;
        structureToPlace = StructureType.NONE; 
        EventManager.Structures.onSetNonePlacementState?.Invoke();
    }
    public void SetPlacingState(StructureType structureType) 
    { 
        placementState = PlacementState.PLACING;
        structureToPlace = structureType;
        EventManager.Structures.onStartPlacementState?.Invoke(structureType); 
    }

    public bool IsNoneState() { return placementState == PlacementState.NONE; }
    public bool IsPlacingState() { return placementState == PlacementState.PLACING; }

    private void OnEnable()
    {
        EventManager.UI.onPressTowerButton += OnPressTowerButton;
        EventManager.Input.onTouchBegan += OnTouchBegan;

    }

    private void OnDisable()
    {
        EventManager.UI.onPressTowerButton -= OnPressTowerButton;
        EventManager.Input.onTouchBegan -= OnTouchBegan;
    }

}
}