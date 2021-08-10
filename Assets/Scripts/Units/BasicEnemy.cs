using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using System;
using BioTower.Level;

namespace BioTower.Units
{
[SelectionBase]
public class BasicEnemy : Unit
{
    [Header("References")]
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private Collider2D triggerCollider;

    
    [Header("Enemy state")]
    [SerializeField] private Color stoppedColor;
    public bool hasCrystal;
    public Color hasCrystalTintColor;
    private bool isRegistered;
    [HideInInspector] public bool isEngagedInCombat;

    [Header("Waypoint movement")]
    [SerializeField] private Waypoint currWaypoint; 
    [SerializeField] private Waypoint nextWaypoint;


    public override void Start()
    {
        base.Start();
        GameManager.Instance.RegisterEnemy(this);
    }

    /// <summary>
    /// Sets the current waypoint, its last waypoint the enemy encountered
    /// </summary>
    /// <param name="waypoint"></param>
    public void SetCurrWaypoint(Waypoint waypoint) { currWaypoint = waypoint; }

    /// <summary>
    /// Sets the next waypoint, its where the enemy is going
    /// </summary>
    /// <param name="waypoint"></param>
    public void SetNextWaypoint(Waypoint waypoint) { nextWaypoint = waypoint; }
    public Waypoint GetCurrWaypoint() { return currWaypoint; }
    public Waypoint GetNextWaypoint() { return nextWaypoint; }

    public void SetSpeed(Vector2 minMaxSpeed, float duration=0)
    {
        float targetSpeed = UnityEngine.Random.Range(minMaxSpeed.x, minMaxSpeed.y);
        if (Mathf.Approximately(duration, 0))
        {
            agent.maxSpeed = targetSpeed;
        }
        else
        {
            LeanTween.value(gameObject, agent.maxSpeed, targetSpeed, 0.5f).setOnUpdate((float val) => {
                agent.maxSpeed = val;
            });
        }
    }
    
    public override void StopMoving()
    {
        agent.Stop();
        //Debug.Log("Stop Moving");
        isEngagedInCombat = true;
        sr.color = stoppedColor;
    }

    public void SetDestination(Waypoint waypoint)
    {
        if (agent == null)
            return;
            
        var randomPoint = UnityEngine.Random.insideUnitSphere * 0.5f;
        randomPoint.z = 0;
        agent.SetDestination(waypoint.transform.position + randomPoint);
    }

    public override void StartMoving(Waypoint waypoint, float delay=0)
    {
        LeanTween.delayedCall(delay, () => {
            SetDestination(waypoint);
            isEngagedInCombat = false;
        });
    }

    // private void LevelLoaded()
    // {
    //     GameManager.Instance.RegisterEnemy(this);
    // }

    private void DestinationReached()
    {
        SetCurrWaypoint(nextWaypoint);
        
        if (currWaypoint.isFork)
        {
            var nextPoint = currWaypoint.ChooseNextWaypoint();
            SetNextWaypoint(nextPoint);
            SetDestination(nextPoint);
        }
        else if (currWaypoint.isEndpoint)
        {
            Debug.Log("Base reached");
            EventManager.Units.onEnemyBaseReached?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            var nextPoint = currWaypoint.nextWaypoint;
            SetNextWaypoint(nextPoint);
            SetDestination(nextPoint);
        }
        
        EventManager.Units.onEnemyReachedDestination?.Invoke(this);
    }

    private void SpawnCrystal()
    {
        var crystalGO = Instantiate(crystalPrefab);
        crystalGO.transform.position = transform.position;

        Vector3 defautlScale = crystalGO.transform.localScale;
        crystalGO.transform.localScale = Vector3.zero;
        LeanTween.scale(crystalGO, defautlScale, 0.1f);
    }

    public override bool TakeDamage(int amount)
    {
        if (base.TakeDamage(amount))
        {
            return isAlive;
        }
        else
        {
            SpawnCrystal();
            triggerCollider.enabled = false;
            return isAlive;
            //DestroyImmediate(gameObject);
            //base.KillUnit();
        }
    }

    private void PickupCrystal(Collider2D col)
    {
        var crystal = col.transform.parent.GetComponent<EnemyCrystal>();

        if (crystal.hasBeenPickedUp)
            return;

        crystal.DestroyObject();
        hasCrystal = true;
        var oldScale = sr.transform.localScale;
        LeanTween.scale(sr.gameObject, oldScale * 1.2f, 0.25f);
        sr.color = hasCrystalTintColor;
        // TODO: make enemy stronger after picking up crystal

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != 13)
            return;

        PickupCrystal(col);
    }

    private void OnTogglePaths()
    {
        LeanTween.delayedCall(0.1f, () => {
            Vector3 targetPos = GameManager.Instance.playerBase.transform.position;
            agent.SetDestination(targetPos);
        });
    }

    private void OnGameStateInit(GameState gameState)
    {
        if (gameState == GameState.GAME_OVER_LOSE || gameState == GameState.GAME_OVER_WIN)
            StopMoving();

    }

    private void OnEnable()
    {
        //EventManager.Game.onLevelLoaded_02 += LevelLoaded;
        agent.OnDestinationReached += DestinationReached;
        EventManager.Game.onTogglePaths += OnTogglePaths;
        EventManager.Game.onGameStateInit += OnGameStateInit;
    }

    private void OnDisable()
    {
        //EventManager.Game.onLevelLoaded_02 -= LevelLoaded;
        agent.OnDestinationReached -= DestinationReached;
        EventManager.Game.onTogglePaths -= OnTogglePaths;
        EventManager.Game.onGameStateInit -= OnGameStateInit;
    }

}
}
