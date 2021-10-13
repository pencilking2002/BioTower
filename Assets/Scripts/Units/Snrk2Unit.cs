using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower.Units
{
public enum SnrkUnitState
{
    IDLE,
    SEARCHING,
    CARRYING_CRYSTAL,
    RETURNING,
    COMBAT,
    DESTROYED
}

public class Snrk2Unit : Unit
{
    public SnrkUnitState snrkUnitState;
    public bool hasCrystalTarget;
    [SerializeField] private EnemyCrystal crystalTarget;

    [Header("References")]
    //public PPC2Tower tower;
    [SerializeField] private GameObject crystalSprite;

    public override void Start()
    {
        base.Start();
        Util.ScaleUpSprite(sr, 1.1f);
        agent.maxSpeed = Util.upgradeSettings.snrk2UnitSpeed_float.GetFloat();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsCombatState() && other.gameObject.layer == 10) 
        {
            var targetEnemy = other.transform.parent.GetComponent<BasicEnemy>();

            if (targetEnemy.isEngagedInCombat)
                return;

            EventManager.Units.onStartCombat?.Invoke(this, targetEnemy);
        }
    }

    public void CheckForCrystals()
    {
        if (hasCrystalTarget)
            return;

        if (Util.crystalManager.HasValidCrystals())
        {
            SetupUnitToSearchForCrystal();
        }
    }


     public void SetupUnitToSearchForCrystal()
    {
        crystalTarget = Util.crystalManager.FindValidCrystal(transform);
        crystalTarget.isTargeted = true;
        hasCrystalTarget = true;
        agent.SetDestination(crystalTarget.transform.position);
        SetSearchingState();
    }

    public override void SetCombatState() { snrkUnitState = SnrkUnitState.COMBAT; } 
    public override void SetRoamingState() { snrkUnitState = SnrkUnitState.SEARCHING; }
    public override void SetDestroyedState() { snrkUnitState = SnrkUnitState.DESTROYED; }
    public override void SetNewDestination() { }
    public override void Deregister() { GetPPC2Tower().RemoveUnit(this); }

    public override bool IsCombatState() { return snrkUnitState == SnrkUnitState.COMBAT; }
    public bool IsRoamingState() { return snrkUnitState == SnrkUnitState.SEARCHING; }
    public bool IsIdleState() { return snrkUnitState == SnrkUnitState.IDLE; }
    public bool IsCarryingCrystalState() { return snrkUnitState == SnrkUnitState.CARRYING_CRYSTAL; }
    public bool IsSearchingState() { return snrkUnitState == SnrkUnitState.SEARCHING; }
    public bool IsReturniningState() { return snrkUnitState == SnrkUnitState.RETURNING; }

    public void SetIdleState() { snrkUnitState = SnrkUnitState.IDLE; }
    public void SetCarryingCrystalState() { snrkUnitState = SnrkUnitState.CARRYING_CRYSTAL; }
    public void SetSearchingState() { snrkUnitState = SnrkUnitState.SEARCHING; }
    public void SetReturningState() { snrkUnitState = SnrkUnitState.RETURNING; }

    private void OnDestinationReached()
    {
        Debug.Log($"Snrk2 unit: Destination reached: hasCrystaltarget: {hasCrystalTarget}. state: {snrkUnitState}");

        if (!hasCrystalTarget)
            return;

        if (IsSearchingState())
        {
            SetCarryingCrystalState();
            crystalTarget.DestroyObject();
            agent.SetDestination(GameManager.Instance.playerBase.transform.position);  
            crystalSprite.SetActive(true);
            EventManager.Units.onCrystalPickedUp?.Invoke(this);
        }
        else if (IsCarryingCrystalState())
        {
            EventManager.Game.onSnrk2UnitReachedBase?.Invoke(this);
            Deregister();
            //Destroy(gameObject);
            KillUnit();
        }
    }

    private void OnCrystalDestroyed(EnemyCrystal crystal)
    {
        if (!hasCrystalTarget || IsCarryingCrystalState())
            return;
        
        //if (IsSearchingState() && hasCrystalTarget && crystalTarget == crystal)
        
        if (IsSearchingState())
        {
            if (crystal == crystalTarget)
            {
                hasCrystalTarget = false;
                crystalTarget = null;
                if (Util.crystalManager.HasValidCrystals())
                {
                    SetupUnitToSearchForCrystal();
                }
                else
                {
                    // What if the tower is destroyed?
                    SetReturningState();
                    agent.SetDestination(tower.transform.position);
                }
            }
        }
    }

    public override void StopMoving()
    {
        agent.Stop();
        anim.SetBool("Walk", false);
    }

    private void OnStructureDestroyed(Structure structure)
    {
        if (structure == tower)
            KillUnit();
    }

    private void OnGameStateInit(GameState gameState)
    {
        if (gameState == GameState.GAME_OVER_LOSE || gameState == GameState.GAME_OVER_WIN)
        {
            agent.Stop();
            anim.SetBool("Walk", false);
        }
    }

    private void OnEnable()
    {
        agent.OnDestinationReached += OnDestinationReached;
        agent.OnDestinationInvalid += OnDestinationReached; // Used for when the destination is inside an obstacle
        EventManager.Game.onCrystalDestroyed += OnCrystalDestroyed;
        EventManager.Structures.onStructureDestroyed += OnStructureDestroyed;
        EventManager.Game.onGameStateInit += OnGameStateInit;        
    }

    private void OnDisable()
    {
        agent.OnDestinationReached -= OnDestinationReached;
        agent.OnDestinationInvalid -= OnDestinationReached;
        EventManager.Game.onCrystalDestroyed -= OnCrystalDestroyed;
        EventManager.Structures.onStructureDestroyed -= OnStructureDestroyed;
        EventManager.Game.onGameStateInit -= OnGameStateInit;
    }
}
}