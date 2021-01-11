﻿using UnityEngine;
using PolyNav;
using BioTower.Structures;

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
    public AbaUnitState abaUnitState;

    [Header("Roaming Settings")]
    public float roamingSpeed = 2.0f;
    [HideInInspector] public bool hasTargetRoamingPoint;

    [Header("Carrying enemy state")]
    [SerializeField] private BasicEnemy carriedEnemy;

    [Header("References")]
    public ABATower abaTower;
    public Rigidbody rb;
    public PolyNavAgent agent;
    
    private void Awake()
    {
        //agent.enabled = false;
        abaUnitState = AbaUnitState.ROAMING;
    }

    public override void Start()
    {
        base.Start();
    }
    
    public void Patrol()
    {
        if (!hasTargetRoamingPoint)
        {
            var targetPoint = abaTower.GetPointWithinInfluence();
            hasTargetRoamingPoint = true;
            var seq = LeanTween.sequence();
            var duration = UnityEngine.Random.Range(1.0f, 2.0f);
            seq.append(
                LeanTween.move(gameObject, targetPoint, duration).setEaseInOutQuad()
            );
            seq.append(() => {
                hasTargetRoamingPoint = false;
            });
        }
    }

    public bool IsRoamingState() { return abaUnitState == AbaUnitState.ROAMING; }
    public bool IsCarryingEnemyState() { return abaUnitState == AbaUnitState.CARRYING_ENEMY; }
    public void SetCarryingEnemyState() { abaUnitState = AbaUnitState.CARRYING_ENEMY; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SetCarryingEnemyState();
        carriedEnemy = other.transform.parent.GetComponent<BasicEnemy>();
        carriedEnemy.StopMoving();
        LeanTween.cancel(gameObject);
        hasTargetRoamingPoint = false;
    }
}
}
