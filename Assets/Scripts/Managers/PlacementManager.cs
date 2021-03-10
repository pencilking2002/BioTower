using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using BioTower.UI;
using System;
using BioTower.Structures;

namespace BioTower
{
public enum PlacementState
{
    NONE,
    PLACING,
}

public class PlacementManager : MonoBehaviour
{
    public static Action<StructureType> onStartPlacementState;
    public static Action onSetNonePlacementState;

    [SerializeField] private PlacementState placementState;
    private StructureType structureToPlace;


    [Header("Structure prefabs")]
    [SerializeField] private GameObject abaTowerPrefab;


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
               
        }
        return tower;
    }

    private void OnPressTowerButton(StructureType structureType)
    {
        SetPlacingState(structureType);   
    }

    private void OnTouchBegan(Vector3 screenPos)
    {
        if (IsPlacingState())
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero);

            if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == 12)
            {
                Debug.Log("Place tower");
                var tower = CreateStructure(structureToPlace);
                tower.transform.position = hitInfo.collider.transform.position;
                
                SetNoneState();
                
            }
        }
    }

    private void OnStartPlacementState(StructureType structureType)
    {
        var tower = CreateStructure(structureType);
    }


    private void SetNoneState() 
    { 
        placementState = PlacementState.NONE;
        structureToPlace = StructureType.NONE; 
        onSetNonePlacementState?.Invoke();
    }
    private void SetPlacingState(StructureType structureType) 
    { 
        placementState = PlacementState.PLACING;
        structureToPlace = structureType;
        onStartPlacementState?.Invoke(structureType); 
    }

    private bool IsNoneState() { return placementState == PlacementState.NONE; }
    private bool IsPlacingState() { return placementState == PlacementState.PLACING; }

    private void OnEnable()
    {
        GameplayUI.onTowerButton += OnPressTowerButton;
        InputController.onTouchBegan += OnTouchBegan;

    }

    private void OnDisable()
    {
        GameplayUI.onTowerButton -= OnPressTowerButton;
        InputController.onTouchBegan -= OnTouchBegan;
    }

}
}