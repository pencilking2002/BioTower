using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;
using BioTower.Units;

namespace BioTower
{
public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    public LevelSettings waveSettings; 
    [SerializeField] private WaveMode waveMode;
    public int currWave;
    [HideInInspector] public bool wavesHaveCompleted;

    private NotStartedState notStartedState;
    private DelayState delayState;
    private InProgressState inProgressState;
    private EndedState endedState;

    private Dictionary<WaveMode, WaveState> waveStateMap = new Dictionary<WaveMode, WaveState>(); 

    private void Awake()
    {
        notStartedState = GetComponent<NotStartedState>();
        delayState = GetComponent<DelayState>();
        inProgressState = GetComponent<InProgressState>();
        endedState = GetComponent<EndedState>();

        waveStateMap.Add(WaveMode.NOT_STARTED, notStartedState); 
        waveStateMap.Add(WaveMode.DELAY, delayState); 
        waveStateMap.Add(WaveMode.IN_PROGRESS, inProgressState); 
        waveStateMap.Add(WaveMode.ENDED, endedState); 

        // Initialize waves
        for (int i=0; i<waveSettings.waves.Length; i++)
            waveSettings.waves[i].Init(i);
        
    }

    public void SpawnEnemy(Vector2 minMaxSpeed)
    {
        var enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.position = GameObject.FindGameObjectWithTag(Constants.enemyTestSpawnSpot).transform.position;
        enemyGO.GetComponent<BasicEnemy>().SetSpeed(minMaxSpeed);
    }

    private void Update()
    {
        if (wavesHaveCompleted)
            return;

        var wave = waveSettings.waves[currWave];
        wave.state = waveStateMap[wave.state].OnUpdate(wave);
        waveMode = wave.state;
    }

}
}