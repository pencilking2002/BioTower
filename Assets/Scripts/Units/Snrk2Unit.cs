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
    CARRYING_CRYSTAL,
    RETURNING
}

public class Snrk2Unit : Unit
{
    public SnrkUnitState snrkUnitState;
    [SerializeField] private List<AbaUnit> abaUnits;
    public bool hasCrystalTarget;
    [SerializeField] private EnemyCrystal crystalTarget;


    [Header("References")]
    public PPC2Tower tower;
    public PolyNavAgent agent;

    public override void Start()
    {
        Util.ScaleUpSprite(sr, 1.1f);
        CheckForCrystals();
    }

    private void CheckForCrystals()
    {
        if (hasCrystalTarget)
            return;

        if (Util.crystalManager.HasValidCrystals())
        {
            SetupUnitToSearchForCrystal();
        }
    }

    private void SetupUnitToSearchForCrystal()
    {
        crystalTarget = Util.crystalManager.FindValidCrystal(transform);
        crystalTarget.isTargeted = true;
        hasCrystalTarget = true;
        agent.SetDestination(crystalTarget.transform.position);
        SetSearchingState();
    }

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
        if (!hasCrystalTarget)
            return;

        if (IsSearchingState())
        {
            crystalTarget.DestroyObject();
            SetCarryingCrystalState();
            agent.SetDestination(GameManager.Instance.playerBase.transform.position);   
        }
        else if (IsCarryingCrystalState())
        {
            EventManager.Game.onSnrk2UnitReachedBase?.Invoke(this);
            tower.RemoveUnit(this);
            Destroy(gameObject);
        }
    }

    private void OnCrystalDestroyed(EnemyCrystal crystal)
    {
        if (!hasCrystalTarget || IsCarryingCrystalState())
            return;
        
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

    private void OnStructureDestroyed(Structure structure)
    {
        if (structure == tower)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        agent.OnDestinationReached += OnDestinationReached;
        agent.OnDestinationInvalid += OnDestinationReached; // Used for when the destination is inside am obstacle
        EventManager.Game.onCrystalDestroyed += OnCrystalDestroyed;
        EventManager.Structures.onStructureDestroyed += OnStructureDestroyed;
    }

    private void OnDisable()
    {
        agent.OnDestinationReached -= OnDestinationReached;
        agent.OnDestinationInvalid -= OnDestinationReached;
        EventManager.Game.onCrystalDestroyed -= OnCrystalDestroyed;
        EventManager.Structures.onStructureDestroyed -= OnStructureDestroyed;
    }

}
}