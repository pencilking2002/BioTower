﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{
public class TapManager : MonoBehaviour
{
    [SerializeField] private LayerMask tappableLayerMask;
    [SerializeField] private LayerMask structureLayerMask;
    public Structure selectedStructure;
    public bool hasSelectedStructure;


    private void OnTouchBegan(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, tappableLayerMask);
        if (hitInfo.collider != null)
        {
            TapCrystal(hitInfo);
            TapLightFragment(hitInfo);
            
        }
        TapStructure(screenPos);
    }

    private void TapCrystal(RaycastHit2D hitInfo)
    {
        EnemyCrystal crystal = hitInfo.collider.transform.parent.GetComponent<EnemyCrystal>();
        if (crystal != null)
        {
            crystal.DestroyObject();
            GameManager.Instance.econManager.GainCrystalMoney();
        }
    }

    private void TapLightFragment(RaycastHit2D hitInfo)
    {
        LightFragment fragment = hitInfo.collider.transform.parent.GetComponent<LightFragment>();
        if (fragment != null)
        {
            fragment.DestroyObject();
            GameManager.Instance.econManager.GainCrystalMoney();
        }
    }

    private void TapStructure(Vector3 screenPos)
    {
        if (GameManager.Instance.placementManager.IsNoneState())
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, structureLayerMask);

            if (hitInfo.collider != null)
            {
                var structure = hitInfo.collider.transform.parent.GetComponent<Structure>();
                hasSelectedStructure = true;
                selectedStructure = structure;
                EventManager.Structures.onStructureSelected?.Invoke(structure);
            }
        }
    }

    private void OnEnable()
    {

        EventManager.Input.onTouchBegan += OnTouchBegan;

    }

    private void OnDisable()
    {

        EventManager.Input.onTouchBegan -= OnTouchBegan;
    }
}
}