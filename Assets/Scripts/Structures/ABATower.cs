﻿using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using PolyNav;

namespace BioTower.Structures
{

[SelectionBase]
public class ABATower : Structure
{   
    [SerializeField] private int numUnitsToSpawn = 4;
    [SerializeField] private List<AbaUnit> abaUnits;


    [Header("References")]
    [SerializeField] private GameObject abaUnitPrefab;
    public PolyNav2D map;
    [SerializeField] private Transform unitsContainer;
    [SerializeField] private CircleCollider2D maxInfluenceAreaCollider;
    [SerializeField] private CircleCollider2D minInfluenceAreaCollider;

    private Vector3[] targetPositions;
 

    public override void Awake()
    {
        base.Awake();
        var unitsContainer = transform.Find("Units");
        SpawnUnits();
        targetPositions = new Vector3[numUnitsToSpawn];
    }

    private void SpawnUnits()
    {
        for(int i=0; i<numUnitsToSpawn; i++)
        {
            var go = Instantiate(abaUnitPrefab);
            go.transform.position = GetPointWithinInfluence();
            go.transform.SetParent(unitsContainer);
            var unit = go.GetComponent<AbaUnit>();
            unit.agent.map = map; 
            unit.abaTower = this;
            AddUnit(unit);
        }
    }

    public void AddUnit(AbaUnit unit)
    {
        abaUnits.Add(unit);
    }

    public void RemoveUnit(AbaUnit unit)
    {
        abaUnits.Remove(unit);
    }

    private void FixedUpdate()
    {
        // for (int i=0; i<numUnitsToSpawn; i++)
        // {
        //     var unit = abaUnits[i];
        //     if (unit.IsRoamingState())
        //     {
        //         unit.Patrol();
        //     }
        // }
    }

    public Vector2 GetPointWithinInfluence()
    {
        var minRadius = minInfluenceAreaCollider.radius * minInfluenceAreaCollider.transform.lossyScale.x;
        var maxRadius = maxInfluenceAreaCollider.radius * maxInfluenceAreaCollider.transform.lossyScale.x;
        Vector2 point = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
        point += (Vector2) minInfluenceAreaCollider.transform.position;
        return point;
    }

    private void OnDrawGizmosSelected()
    {
        var minRadius = minInfluenceAreaCollider.radius * minInfluenceAreaCollider.transform.lossyScale.x;
        var maxRadius = maxInfluenceAreaCollider.radius * maxInfluenceAreaCollider.transform.lossyScale.x;
        
        var color = Color.red;
        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawSphere(minInfluenceAreaCollider.transform.position, minRadius);

        color = Color.blue;
        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawSphere(maxInfluenceAreaCollider.transform.position, maxRadius);
    }
}
}

