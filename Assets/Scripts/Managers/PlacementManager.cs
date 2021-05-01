﻿using System.Collections;
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
    // public static Action<StructureType> onStartPlacementState;
    // public static Action onSetNonePlacementState;

    [SerializeField] private PlacementState placementState;
    [SerializeField] private LayerMask socketLayerMask;
    [SerializeField] private LayerMask structureLayerMask;
    private StructureType structureToPlace;
    [SerializeField] private Vector3 placementOffset;


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
            if (GameManager.Instance.econManager.CanBuyAbaTower())
            {
                PlaceAbaTower(screenPos);
                GameManager.Instance.econManager.BuyAbaTower();
            }
        }
        else
        {
            if (GameManager.Instance.econManager.CanBuyAbaUnit())
            {
                TapStructure(screenPos);
                GameManager.Instance.econManager.BuyAbaUnit();
            }
        }
    }

    private void PlaceAbaTower(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, socketLayerMask);
        //Debug.Log(hitInfo.collider.gameObject.name);
        if (hitInfo.collider != null)
        {
            var socket = hitInfo.collider.transform.parent.GetComponent<StructureSocket>();
            if (!socket.HasStructure())
            {
                socket.SetHasStructure(true);
                var tower = CreateStructure(structureToPlace);
                tower.transform.position = hitInfo.collider.transform.position + placementOffset;
                SetNoneState();
            }
        }
    }

    private void TapStructure(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, structureLayerMask);

        if (hitInfo.collider != null)
        {
            var structure = hitInfo.collider.transform.parent.GetComponent<Structure>();
            structure?.OnTapStructure();
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
        EventManager.Structures.onSetNonePlacementState?.Invoke();
    }
    private void SetPlacingState(StructureType structureType) 
    { 
        placementState = PlacementState.PLACING;
        structureToPlace = structureType;
        EventManager.Structures.onStartPlacementState?.Invoke(structureType); 
    }

    private bool IsNoneState() { return placementState == PlacementState.NONE; }
    private bool IsPlacingState() { return placementState == PlacementState.PLACING; }

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