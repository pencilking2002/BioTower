using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;

namespace BioTower.Structures
{

[SelectionBase]
public class ABATower : Structure
{   
    [SerializeField] private GameObject abaUnitPrefab;
    [SerializeField] private Transform unitsContainer;
    [SerializeField] private CircleCollider2D maxInfluenceAreaCollider;
    [SerializeField] private CircleCollider2D minInfluenceAreaCollider;
    [SerializeField] private int numUnitsToSpawn = 4;
    [SerializeField] private List<Unit> abaUnits;

    public override void Awake()
    {
        base.Awake();
        var unitsContainer = transform.Find("Units");
        SpawnUnits();
    }

    private void SpawnUnits()
    {
        for(int i=0; i<numUnitsToSpawn; i++)
        {
            var go = Instantiate(abaUnitPrefab);
            go.transform.position = GetPointWithinInfluence();
            go.transform.SetParent(unitsContainer);
            var unit = go.GetComponent<Unit>();
            abaUnits.Add(unit);
        }
    }

    private Vector2 GetPointWithinInfluence()
    {
        Vector2 center = minInfluenceAreaCollider.transform.position;
        var randomDirection = (Random.insideUnitCircle * center).normalized;
        var randomDistance = Random.Range(minInfluenceAreaCollider.radius, maxInfluenceAreaCollider.radius);
        var point = center + randomDirection * randomDistance;
        return point;
    }
}
}

