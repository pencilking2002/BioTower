using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

namespace BioTower.Units
{
[SelectionBase]
public class BasicEnemy : Unit
{
    [SerializeField] private Transform target;
    [SerializeField] private PolyNavAgent agent;

    private void Awake()
    {
    
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        agent.SetDestination(target.position);
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
        GameManager.onLevelLoaded_02 += LevelLoaded;
    }
}
}
