using System.Collections.Generic;
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
    public PolyNav2D roamingMap;
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
        roamingMap = maxInfluenceAreaCollider.GetComponent<PolyNav2D>();
    }

    private void SpawnUnits()
    {
        for(int i=0; i<numUnitsToSpawn; i++)
        {
            var go = Instantiate(abaUnitPrefab);
            go.transform.position = GetPointWithinInfluence();
            go.transform.SetParent(unitsContainer);
            var unit = go.GetComponent<AbaUnit>();

            // test
            //unit.agent.map = roamingMap;
            //var point = GetPointWithinInfluence();
            //unit.destinationPoint = point;
            //unit.agent.map = roamingMap;
            // unit.agent.SetDestination(point);

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

