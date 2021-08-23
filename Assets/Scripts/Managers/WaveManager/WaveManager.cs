using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;
using BioTower.Units;
using BioTower.SaveData;

namespace BioTower
{
public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    public LevelSettings waveSettings => LevelInfo.current.waveSettings;
    [SerializeField] private WaveMode waveMode;
    public int currWave;
    [HideInInspector] public bool wavesInitialized;
    [HideInInspector] public bool wavesHaveCompleted;

    private NotStartedState notStartedState;
    private DelayState delayState;
    private InProgressState inProgressState;
    private EndedState endedState;

    private Dictionary<WaveMode, WaveState> waveStateMap = new Dictionary<WaveMode, WaveState>(); 

    private void Start()
    {
        notStartedState = GetComponent<NotStartedState>();
        delayState = GetComponent<DelayState>();
        inProgressState = GetComponent<InProgressState>();
        endedState = GetComponent<EndedState>();

        waveStateMap.Add(WaveMode.NOT_STARTED, notStartedState); 
        waveStateMap.Add(WaveMode.DELAY, delayState); 
        waveStateMap.Add(WaveMode.IN_PROGRESS, inProgressState); 
        waveStateMap.Add(WaveMode.ENDED, endedState); 
    }

    public void SpawnEnemy(Vector2 minMaxSpeed)
    {
        // Initialize enemy
        var enemyGO = Instantiate(enemyPrefab);
        var enemy = enemyGO.GetComponent<BasicEnemy>();

        // Set the enemy's positioning
        var spawnPoint = GameManager.Instance.GetWaypointManager().GetRandomSpawnPoint();
        enemyGO.transform.position = spawnPoint.transform.position;
        enemy.SetCurrWaypoint(spawnPoint);
        enemy.SetNextWaypoint(spawnPoint.nextWaypoint);

        // Setup enemy
        GameManager.Instance.RegisterEnemy(enemy);
        enemy.SetSpeed(minMaxSpeed);
        enemy.StartMoving(enemy.GetNextWaypoint());
    }

    private void Update()
    {
        if (!GameManager.Instance.gameStates.IsGameState())
            return;

        if (!wavesInitialized)
            return;

        if (wavesHaveCompleted)
            return;

        var wave = waveSettings.waves[currWave];
        wave.state = waveStateMap[wave.state].OnUpdate(wave);
        waveMode = wave.state;
    }

    private void OnEnemyReachedDestination(BasicEnemy enemy)
    {
        // Vary speed
        var minMaxSpeed = waveSettings.waves[currWave].minMaxSpeed;
        enemy.SetSpeed(minMaxSpeed, 0.5f);
    }

    private void OnLevelStart(int levelIndex)
    {
        // Don't automatically start waves on the first level
        // because there's a tutorial
        if (levelIndex == 0)
            return;

        // Initialize waves
        for (int i=0; i<waveSettings.waves.Length; i++)
            waveSettings.waves[i].Init(i);
        
        wavesInitialized = true;
        
    }

    private void OnEnable()
    {
        EventManager.Units.onEnemyReachedDestination += OnEnemyReachedDestination;
        EventManager.Game.onLevelAwake += OnLevelStart;
    }

    private void OnDisable()
    {
        EventManager.Units.onEnemyReachedDestination -= OnEnemyReachedDestination;
        EventManager.Game.onLevelAwake -= OnLevelStart;
    }

}
}