using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using BioTower.UI;

namespace BioTower
{
public enum PlacementState
{
    NONE,
    PLACING,
}

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private Transform reticleTransform;
    [SerializeField] private PlacementState placementState;
    private Vector3 offscreenPos = new Vector3(1000,1000,0);


    private void Awake()
    {
        reticleTransform.position = offscreenPos;
    }

    private void Update()
    {
       
    }


    private void SetNoneState() { placementState = PlacementState.NONE; }
    private void SetPlacingState() { placementState = PlacementState.PLACING; }

    private bool IsNoneState() { return placementState == PlacementState.NONE; }
    private bool IsPlacingState() { return placementState == PlacementState.PLACING; }

    private void OnPressTowerButton(StructureType structureType)
    {
        if (structureType == StructureType.ABA_TOWER)
        {
            reticleTransform.position = Vector3.zero;
            SetPlacingState();
        }
    }

    private void OnTouchBegan(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero);

        if (hitInfo.collider != null && hitInfo.collider.gameObject.name == "SpawnReticle")
        {
            Debug.Log("Hit spawn reticle");
        }
               
    }

    private void OnEnable()
    {
        GameplayUI.onTowerButton += OnPressTowerButton;
        InputController.onTouchBegan += OnTouchBegan;
    }

    private void OnDisable()
    {
        GameplayUI.onTowerButton -= OnPressTowerButton;
        InputController.onTouchBegan += OnTouchBegan;
    }

}
}