using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;

namespace BioTower.Structures
{

[SelectionBase]
public class ABATower : Structure
{   

    [SerializeField] private CircleCollider2D maxInfluenceAreaCollider;
    [SerializeField] private CircleCollider2D minInfluenceAreaCollider;
    [SerializeField] private int numUnitsToSpawn = 4;
    [SerializeField] private List<AbaUnit> abaUnits;

    [Header("References")]
    [SerializeField] private GameObject abaUnitPrefab;
    [SerializeField] private Transform unitsContainer;

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
            unit.abaTower = this;
            abaUnits.Add(unit);
        }
    }

    private void FixedUpdate()
    {
        for (int i=0; i<numUnitsToSpawn; i++)
        {
            var unit = abaUnits[i];
            if (unit.IsRoamingState())
            {
                unit.Patrol();
            }
        }
    }

    public Vector2 GetPointWithinInfluence()
    {
        Vector2 point = Random.insideUnitCircle.normalized * Random.Range(minInfluenceAreaCollider.radius, maxInfluenceAreaCollider.radius);
        point += (Vector2) minInfluenceAreaCollider.transform.position;
        return point;
    }
}
}

