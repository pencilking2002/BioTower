using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using System;

namespace BioTower.Units
{
[SelectionBase]
public class BasicEnemy : Unit
{
    public static Action onBaseReached;
    public PolyNavAgent agent;
    private bool isRegistered;

    public override void Awake()
    {
        base.Awake();
        GameManager.Instance.RegisterEnemy(this);
    }

    // public void SetTarget(Transform target)
    // {
    //     this.target = target;
    //     agent.SetDestination(target.position);
    // }

    public void SetMap(PolyNav2D map)
    {
        agent.map = map;
    }

    public void StopMoving()
    {
        agent.Stop();
    }

    private void LevelLoaded()
    {
        GameManager.Instance.RegisterEnemy(this);
    }

    private void DestinationReached()
    {
        Debug.Log("Base reached");
        onBaseReached?.Invoke();
    }

    private void OnEnable()
    {
        GameManager.onLevelLoaded_02 += LevelLoaded;
        agent.OnDestinationReached += DestinationReached;
    }

    private void OnDisable()
    {
        GameManager.onLevelLoaded_02 -= LevelLoaded;
        agent.OnDestinationReached -= DestinationReached;
    }
}
}
