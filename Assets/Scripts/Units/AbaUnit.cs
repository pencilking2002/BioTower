using UnityEngine;
using BioTower.Structures;
using System;

namespace BioTower.Units
{

public enum AbaUnitState
{
    ROAMING,
    COMBAT,
    CARRYING_ENEMY,
    DESTROYED,
    CHASING_ENEMY
}

[SelectionBase]
public class AbaUnit : Unit
{
    public AbaUnitState abaUnitState;

    [Header("Combat enemy state")]
    public BasicEnemy targetEnemy;
    [SerializeField] private float combatDuration = 2.0f;
    [Range(0, 100)][SerializeField] private float abaWinChance = 50;


    [Header("References")]
    //public ABATower abaTower;
    public Rigidbody rb;
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

        agent.maxSpeed = Util.upgradeSettings.abaUnitMaxSpeed_float.GetFloat();
        
    }
    
    public override void StopMoving()
    {
        agent.Stop();
        agent.enabled = false;
        //Debug.Log("AbaUnit: Stop Moving");
        anim.SetBool("Walk", false);
        //sr.color = stoppedColor;
    }

    public bool IsRoamingState() 
    {
        agent.enabled = true; 
        return abaUnitState == AbaUnitState.ROAMING; 
    }
    public bool IsCarryingEnemyState() { return abaUnitState == AbaUnitState.CARRYING_ENEMY; }
    public override bool IsCombatState() { return abaUnitState == AbaUnitState.COMBAT; }
    public bool IsDestroyedState() { return abaUnitState == AbaUnitState.DESTROYED; }
    public bool IsChasingState() { return abaUnitState == AbaUnitState.CHASING_ENEMY; }

    public override void SetRoamingState() 
    { 
        abaUnitState = AbaUnitState.ROAMING; 
    }

    public void SetCarryingEnemyState() { abaUnitState = AbaUnitState.CARRYING_ENEMY; }
    public override void SetCombatState() 
    { 
        abaUnitState = AbaUnitState.COMBAT;
        anim.SetBool("Attack", true);
    } 
    public override void SetDestroyedState() 
    { 
        abaUnitState = AbaUnitState.DESTROYED;
        StopMoving();
        isAlive = false;
        anim.SetBool("Dead", true);
        anim.SetBool("Attack", false); 
        GameManager.Instance.unitManager.Unregister(this);
        Deregister();
        healthSlider.gameObject.SetActive(false);

        // after 5 sec, make unit scale down and destroy it
        LeanTween.delayedCall(gameObject, 5, () => {
            LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(() => {
                Destroy(gameObject);
            });
        });
    }
    public void SetChasingState() { abaUnitState = AbaUnitState.CHASING_ENEMY; }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsCombatState() && other.gameObject.layer == 10 && isAlive) 
        {
            targetEnemy = other.transform.parent.GetComponent<BasicEnemy>();

            if (targetEnemy.isEngagedInCombat)
                return;

            EventManager.Units.onStartCombat?.Invoke(this, targetEnemy);
        }
    }

    public override void SetNewDestination()
    {
        agent.enabled = true;
        if (agent == null)
            return;

        var newDestination = GetAbaTower().GetEdgePointWithinInfluence();
        agent.SetDestination(newDestination);
        anim.SetBool("Walk", true);
        anim.SetBool("Attack", false);
//        Debug.Log("Set destination");
        //Debug.Log("AbaUnit: Set New Destination: " + newDestination);
    }

    private void OnDestinationReached()
    {
        if (IsRoamingState())
        {
            SetNewDestination();
        }
    }

    public override void Deregister()
    {
        GetAbaTower().RemoveUnit(this);
    }

    public override void KillUnit() 
    { 
        SetDestroyedState();
        EventManager.Units.onUnitDestroyed?.Invoke(this);
    }
    
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
