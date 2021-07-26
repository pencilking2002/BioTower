using UnityEngine;
using PolyNav;
using BioTower.Structures;
using System;

namespace BioTower.Units
{

public enum AbaUnitState
{
    ROAMING,
    COMBAT,
    CARRYING_ENEMY,
    DESTROYED
}

[SelectionBase]
public class AbaUnit : Unit
{
    public AbaUnitState abaUnitState;

    [Header("Combat enemy state")]
    [SerializeField] private BasicEnemy targetEnemy;
    [SerializeField] private float combatDuration = 2.0f;
    [Range(0, 100)][SerializeField] private float abaWinChance = 50;


    [Header("References")]
    public ABATower abaTower;
    public Rigidbody rb;
    public PolyNavAgent agent;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        abaUnitState = AbaUnitState.ROAMING;
    }

    public override void Start()
    {
        base.Start();
        Util.ScaleUpSprite(sr, 1.1f);
        SetRoamingState();
        SetNewDestination();
    }
    
    public override void StopMoving()
    {
        agent.Stop();
        Debug.Log("AbaUnit: Stop Moving");
        anim.SetBool("Walk", false);
        //sr.color = stoppedColor;
    }

    public bool IsRoamingState() { return abaUnitState == AbaUnitState.ROAMING; }
    public bool IsCarryingEnemyState() { return abaUnitState == AbaUnitState.CARRYING_ENEMY; }
    public bool IsCombatState() { return abaUnitState == AbaUnitState.COMBAT; }
    public bool IsDestroyedState() { return abaUnitState == AbaUnitState.DESTROYED; }

    public void SetRoamingState() { abaUnitState = AbaUnitState.ROAMING; }
    public void SetCarryingEnemyState() { abaUnitState = AbaUnitState.CARRYING_ENEMY; }
    public void SetCombatState() { abaUnitState = AbaUnitState.COMBAT; } 
    public bool SetDestroyedState() { return abaUnitState == AbaUnitState.DESTROYED; }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsCombatState() && other.gameObject.layer == 10) 
        {
            targetEnemy = other.transform.parent.GetComponent<BasicEnemy>();

            if (targetEnemy.isEngagedInCombat)
                return;

            EventManager.Units.onStartCombat?.Invoke(this, targetEnemy);
        }
    }

    public void SetNewDestination()
    {
        var newDestination = abaTower.GetEdgePointWithinInfluence();
        agent.SetDestination(newDestination);
        anim.SetBool("Walk", true);
        Debug.Log("Set destination");
        //Debug.Log("AbaUnit: Set New Destination: " + newDestination);
    }

    private void OnDestinationReached()
    {
        if (IsRoamingState())
        {
            SetNewDestination();
        }
    }

    // public override void KillUnit()
    // {
    //     base.KillUnit();
    // }

    private void OnEnable()
    {
        agent.OnDestinationReached += OnDestinationReached;
        agent.OnDestinationInvalid += OnDestinationReached; // Used for when the destination is inside am obstacle
    }

    private void OnDisable()
    {
        agent.OnDestinationReached -= OnDestinationReached;
        agent.OnDestinationInvalid -= OnDestinationReached;
    }
}
}
