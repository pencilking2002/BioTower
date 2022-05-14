﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BioTower.Units;

namespace BioTower
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private GameObject basicEnemyPrefab;
        [SerializeField] private GameObject midEnemyPrefab;
        [SerializeField] private GameObject advancedEnemyPrefab;

        public WaveSettings waveSettings => LevelInfo.current.waveSettings;
        [SerializeField] private WaveMode waveMode;
        private Dictionary<UnitType, GameObject> enemyDict = new Dictionary<UnitType, GameObject>();

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

        private void Awake()
        {
            enemyDict.Add(UnitType.BASIC_ENEMY, basicEnemyPrefab);
            enemyDict.Add(UnitType.MID_ENEMY, midEnemyPrefab);
            enemyDict.Add(UnitType.ADVANCED_ENEMY, advancedEnemyPrefab);
        }

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
            for (int i = 0; i < waveSettings.waves.Length; i++)
                waveSettings.waves[i].Init(i);
        }

        public GameObject GetEnemyPrefab(UnitType unitType) { return enemyDict[unitType]; }

        public EnemyUnit SpawnEnemy(UnitType enemyType)
        {
            // Initialize enemy
            var enemyPrefab = GetEnemyPrefab(enemyType);
            var enemyGO = Instantiate(enemyPrefab);
            var enemy = enemyGO.GetComponent<EnemyUnit>();

            // Set the enemy's positioning
            var spawnPoint = GameManager.Instance.GetWaypointManager().GetSpawnPoint(currWave.waypointIndex);
            enemyGO.transform.position = spawnPoint.transform.position;
            enemy.SetCurrWaypoint(spawnPoint);
            enemy.SetNextWaypoint(spawnPoint.nextWaypoint);

            // Setup enemy
            GameManager.Instance.RegisterEnemy(enemy);
            //enemy.SetSpeed(minMaxSpeed);
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

            waveMode = waveStateMap[waveMode].OnUpdate(waveMode);
        }

        public void SetEndedState()
        {
            wavesInitialized = false;
            waveMode = WaveMode.ENDED;
        }

        public void SetNotStartedState()
        {
            waveMode = WaveMode.NOT_STARTED;
        }

        private void OnEnemyReachedDestination(EnemyUnit enemy)
        {
            // Vary speed
            enemy.SetSpeed(enemy.minMaxSpeed, 0.5f);
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
            // Debug.Log($"Has tuts: {LevelInfo.current.HasTutorials()}");
            // Debug.Log($"Is last tut: {Util.tutCanvas.IsLastTutorial(data)}");

            if (LevelInfo.current.HasTutorials() && (Util.tutCanvas.IsLastTutorial(data) || Util.tutCanvas.skipTutorials))
            {
                Debug.Log("init waves");
                InitializeWaves();
            }
        }

        private void OnWavesCompleted()
        {
            wavesHaveCompleted = true;
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