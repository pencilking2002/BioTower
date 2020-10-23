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

    public override void Start()
    {
        base.Start();
        GameManager.Instance.RegisterEnemy(this);
    }
    
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
