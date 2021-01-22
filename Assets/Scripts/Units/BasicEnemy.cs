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

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color stoppedColor;
    public PolyNavAgent agent;
    private bool isRegistered;
    [HideInInspector] public bool isBeingCarried;

    public override void Start()
    {
        base.Start();
        GameManager.Instance.RegisterEnemy(this);
    }
    
    public void StopMoving()
    {
        agent.Stop();
        Debug.Log("Stop Moving");
        isBeingCarried = true;
        sr.color = stoppedColor;
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
