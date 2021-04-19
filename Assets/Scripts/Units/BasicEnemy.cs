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

    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color stoppedColor;
    public PolyNavAgent agent;
    private bool isRegistered;
    [HideInInspector] public bool isEngagedInCombat;

    public override void Start()
    {
        base.Start();
        GameManager.Instance.RegisterEnemy(this);
    }
    
    public override void StopMoving()
    {
        agent.Stop();
        Debug.Log("Stop Moving");
        isEngagedInCombat = true;
        sr.color = stoppedColor;
    }

    public override void StartMoving(float delay=0)
    {
        LeanTween.delayedCall(delay, () => {
            agent.SetDestination(GameManager.Instance.playerBase.transform.position);
            isEngagedInCombat = false;
        });
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

    private void SpawnCrystal()
    {
        var crystalGO = Instantiate(crystalPrefab);
        crystalGO.transform.position = transform.position;
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

    public override void KillUnit()
    {
        SpawnCrystal();
        base.KillUnit();
    }
}
}
