using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BioTower.Units;

namespace BioTower.Structures
{

[SelectionBase]
public class ABATower : Structure
{   
    [SerializeField] private int numUnitsToSpawn = 4;
    [SerializeField] private GameObject abaUnitPrefab;
    [SerializeField] private CircleCollider2D influenceAreaCollider;
    [SerializeField] private List<Unit> abaUnits;

    public override void Awake()
    {
        base.Awake();
        
        for(int i=0; i<numUnitsToSpawn; i++)
        {
            var go = Instantiate(abaUnitPrefab);
            Vector3 circlePos = UnityEngine.Random.insideUnitCircle * influenceAreaCollider.radius;
            go.transform.position = influenceAreaCollider.transform.position + circlePos;  
            go.transform.SetParent(transform);
            var unit = go.GetComponent<Unit>();
            abaUnits.Add(unit);
        }
    }
}
}

