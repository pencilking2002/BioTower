using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using PolyNav;

public class AgentAvoidTest : MonoBehaviour
{
    public GameObject abaUnitPrefab;
    public PolyNav2D map;
    public CircleCollider2D outerCollider;
    public CircleCollider2D innerCollider;
    public int numUnitsToSpawn = 3;
    public float innerRadius = 0.1f;
    public float changeDestinationInterval = 2.0f;
    private float lastChange;
    private List<AbaUnit> abaUnits = new List<AbaUnit>();
    

    private void Awake()
    {
        for (int i=0; i<numUnitsToSpawn; i++)
        {
            var go = Instantiate(abaUnitPrefab);
            go.transform.position = GetPointWithinInfluence(innerCollider.radius, outerCollider.radius);
            var unit = go.GetComponent<AbaUnit>();
            var newDestination = GetPointWithinInfluence(outerCollider.radius, innerRadius);
            unit.agent.SetDestination(newDestination);
            unit.agent.map = map;
            abaUnits.Add(unit);
        }
    }

    private void Update()
    {
        if (Time.time > lastChange + changeDestinationInterval)
        {
            for (int i=0; i<numUnitsToSpawn; i++)
            {
                var newDestination = GetPointWithinInfluence(outerCollider.radius, innerRadius);
                abaUnits[i].agent.SetDestination(newDestination);
            }
            lastChange = Time.time;
        }
    }

    public Vector2 GetPointWithinInfluence(float innerRadius, float outerRadius)
    {
        Vector2 point = Random.insideUnitCircle.normalized * Random.Range(innerRadius, outerRadius);
        point += (Vector2) transform.position;
        return point;
    }


}
