using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BioTower.Units;

namespace BioTower
{
public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    public LevelSettings waveSettings => LevelInfo.current.waveSettings;
    [SerializeField] private WaveMode waveMode;
    //public int currWave => waveSettings.wa
    public Wave currWave
    {
        get
        {
            return waveSettings.waves[currWaveIndex];
        }
    }
    public int currWaveIndex;
    public bool wavesInitialized;
    public bool wavesHaveCompleted;

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

    
    private void InitializeWaves()
    {
        currWaveIndex = 0;
        wavesInitialized = true;
        wavesHaveCompleted = false;

        // Initialize waves
        for (int i=0; i<waveSettings.waves.Length; i++)
            waveSettings.waves[i].Init(i);
    }

    public BasicEnemy SpawnEnemy(Vector2 minMaxSpeed)
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
        return enemy;
    }

    private void Update()
    {
        if (!GameManager.Instance.gameStates.IsGameState())
            return;

        if (!wavesInitialized)
            return;

        if (wavesHaveCompleted)
            return;

        //var wave = waveSettings.waves[currWave];
        waveMode = waveStateMap[waveMode].OnUpdate(waveMode);
        //wave.state = waveMode;
    }

    public void SetEndedState()
    {
        waveMode = WaveMode.ENDED;
    }

    public void SetNotStartedState()
    {
        waveMode = WaveMode.NOT_STARTED;
    }

    private void OnEnemyReachedDestination(BasicEnemy enemy)
    {
        // Vary speed
        var minMaxSpeed = currWave.minMaxSpeed;
        enemy.SetSpeed(minMaxSpeed, 0.5f);
    }   

    

    private void OnLevelStart(LevelType levelType)
    {
        if (LevelInfo.current.HasTutorials())
            return;

        InitializeWaves();
    }

    private void OnTutorialEnd(TutorialData data)
    {
        int index = Array.IndexOf(Util.tutCanvas.tutorials, data);
        //Debug.Log($"Is last tut: {data.name}. Tut index: {index}. Total tuts: {Util.tutCanvas.tutorials.Length}");
        Debug.Log($"Has tuts: {LevelInfo.current.HasTutorials()}");
        Debug.Log($"Is last tut: {GameManager.Instance.currTutCanvas.IsLastTutorial(data)}");

        if (LevelInfo.current.HasTutorials() && 
            GameManager.Instance.currTutCanvas.IsLastTutorial(data))
        {
            Debug.Log("init waves");
            InitializeWaves(); 
        }
    }

    private void OnWavesCompleted()
    {
        wavesInitialized = false;
        currWaveIndex = 0;
    }

    private void OnEnable()
    {
        EventManager.Units.onEnemyReachedDestination += OnEnemyReachedDestination;
        EventManager.Game.onLevelStart += OnLevelStart;
        EventManager.Tutorials.onTutorialEnd += OnTutorialEnd;
        EventManager.Game.onWavesCompleted += OnWavesCompleted;
    }

    private void OnDisable()
    {
        EventManager.Units.onEnemyReachedDestination -= OnEnemyReachedDestination;
        EventManager.Game.onLevelStart -= OnLevelStart;
        EventManager.Tutorials.onTutorialEnd -= OnTutorialEnd;
        EventManager.Game.onWavesCompleted -= OnWavesCompleted;
    }

}
}