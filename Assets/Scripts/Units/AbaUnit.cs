using UnityEngine;
using PolyNav;

namespace BioTower.Units
{

public enum AbaUnitState
{
    ROAMING,
    CARRYING_ENEMY
}

[SelectionBase]
public class AbaUnit : Unit
{
    public float roamingSpeed = 2.0f;
    public PolyNavAgent agent;
    public AbaUnitState abaUnitState;
    public Rigidbody rb;
    [HideInInspector] public bool hasTargetRoamingPoint;
    
    private void Awake()
    {
        //agent.enabled = false;
        abaUnitState = AbaUnitState.ROAMING;
    }

    public override void Start()
    {
        base.Start();
    }
}
}
