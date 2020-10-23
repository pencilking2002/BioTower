using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

namespace BioTower.Units
{
[SelectionBase]
public class BasicEnemy : Unit
{
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


    public void StopMoving()
    {
        agent.Stop();
        Debug.Log("Stop Moving");
    }

    private void LevelLoaded()
    {
        GameManager.Instance.RegisterEnemy(this);
    }

    private void OnEnable()
    {
        GameManager.onLevelLoaded_02 += LevelLoaded;
    }

    private void OnDisable()
    {
        GameManager.onLevelLoaded_02 -= LevelLoaded;
    }
}
}
