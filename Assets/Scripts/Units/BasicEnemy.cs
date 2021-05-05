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
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color stoppedColor;
    public bool hasCrystal;
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
            Vector3 targetPos = GameManager.Instance.playerBase.transform.position;
            agent.SetDestination(targetPos);
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
        EventManager.Units.onEnemyBaseReached?.Invoke();
    }

    private void SpawnCrystal()
    {
        var crystalGO = Instantiate(crystalPrefab);
        crystalGO.transform.position = transform.position;

        Vector3 defautlScale = crystalGO.transform.localScale;
        crystalGO.transform.localScale = Vector3.zero;
        LeanTween.scale(crystalGO, defautlScale, 0.1f);
    }

    public override void KillUnit()
    {
        SpawnCrystal();
        base.KillUnit();
    }

    private void PickupCrystal(Collider2D col)
    {
        var crystal = col.transform.parent.GetComponent<EnemyCrystal>();

        if (crystal.hasBeenPickedUp)
            return;

        crystal.DestroyCrystal();
        hasCrystal = true;
        // TODO: make enemy stronger after picking up crystal
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != 13)
            return;

        PickupCrystal(col);
    }

    private void OnEnable()
    {
        EventManager.Game.onLevelLoaded_02 += LevelLoaded;
        agent.OnDestinationReached += DestinationReached;
    }

    private void OnDisable()
    {
        EventManager.Game.onLevelLoaded_02 -= LevelLoaded;
        agent.OnDestinationReached -= DestinationReached;
    }

}
}
