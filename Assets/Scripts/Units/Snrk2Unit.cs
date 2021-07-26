using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using BioTower.Structures;

namespace BioTower.Units
{
public enum SnrkUnitState
{
    IDLE,
    SEARCHING,
    CARRYING_CRYSTAL
}

public class Snrk2Unit : Unit
{
    public SnrkUnitState snrkUnitState;
    [SerializeField] private List<AbaUnit> abaUnits;


    [Header("References")]
    // [SerializeField] private GameObject abaUnitPrefab;
    // [SerializeField] private Transform unitsContainer;
    public PPC2Tower tower;
    public PolyNavAgent agent;

    public override void Start()
    {
        Util.ScaleUpSprite(sr, 1.1f);
        SetIdleState();
    }

    // public override void SpawnUnits(int numUnits)
    // {
    //     for(int i=0; i<numUnits; i++)
    //     {
    //         var go = Instantiate(abaUnitPrefab);
    //         //go.transform.position = GetPointWithinInfluence();
    //         go.transform.position = GetEdgePointWithinInfluence(); 
                
    //         go.transform.SetParent(unitsContainer);
    //         var unit = go.GetComponent<Snrk2Unit>();
    //         unit.agent.map = map; 
    //         unit.abaTower = this;
    //         AddUnit(unit);
    //     }
    // }

    // public Vector2 GetEdgePointWithinInfluence()
    // {
    //     var point = Util.GetPointWithinInfluence(
    //             minInfluenceAreaCollider.transform.position, 
    //             minInfluenceAreaCollider.radius, 
    //             maxInfluenceAreaCollider.radius
    //         );
    //     Vector2 center = minInfluenceAreaCollider.transform.position;
    //     Vector2 direction = (point-center).normalized;
    //     point = center + direction * maxInfluenceAreaCollider.radius;
    //     return point;
    // }

    // public void AddUnit(AbaUnit unit)
    // {
    //     abaUnits.Add(unit);
    // }


    public bool IsIdleState() { return snrkUnitState == SnrkUnitState.IDLE; }
    public bool IsCarryingCrystalState() { return snrkUnitState == SnrkUnitState.CARRYING_CRYSTAL; }
    public bool IsSearchingState() { return snrkUnitState == SnrkUnitState.SEARCHING; }

    public void SetIdleState() { snrkUnitState = SnrkUnitState.IDLE; }
    public void SetCarryingCrystalState() { snrkUnitState = SnrkUnitState.CARRYING_CRYSTAL; }
    public void SetSearchingState() { snrkUnitState = SnrkUnitState.SEARCHING; }

}
}