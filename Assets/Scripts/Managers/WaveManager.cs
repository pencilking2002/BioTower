using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

namespace BioTower
{
public class WaveManager : MonoBehaviour
{
    public static Action onWavesCompleted;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private LevelSettings waveSettings; 
    public int currWave;
    private bool wavesHaveCompleted;

    private Dictionary<WaveState, Action<Wave>> waveState = new Dictionary<WaveState, Action<Wave>>(); 
    private void Awake()
    {
        waveState.Add(WaveState.NOT_STARTED, NotStarted); 
        waveState.Add(WaveState.DELAY, Delay); 
        waveState.Add(WaveState.IN_PROGRESS, InProgress); 
        waveState.Add(WaveState.ENDED, Ended); 

        // Setup wave states
        foreach(var wave in waveSettings.waves)
        {
            wave.SetNotStarted();
            wave.lastSpawn = 0;
            wave.timeStarted = 0;
        }
    }

    private void SpawnEnemy()
    {
        var enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.position = GameObject.FindGameObjectWithTag(Constants.enemyTestSpawnSpot).transform.position;
        Debug.Log("Spawn enemy");
    }

    private void Update()
    {
        if (wavesHaveCompleted)
            return;

        var wave = waveSettings.waves[currWave];
        waveState[wave.state](wave);
    }

    private void NotStarted(Wave wave)
    {
        wave.timeStarted = Time.time;
        wave.SetDelay();
    }

    private void Delay(Wave wave)
    {
        if (Time.time > wave.timeStarted + wave.startDelay)
        {
            wave.SetInProgress();
        }
    }

    private void InProgress(Wave wave)
    {
        if (Time.time > wave.lastSpawn + wave.spawnInterval)
        {
            SpawnEnemy();
            wave.lastSpawn = Time.time;
        }

        if (Time.time > wave.timeStarted + wave.startDelay + wave.duration)
        {
            wave.SetEnded();    
        }
    }

    private void Ended(Wave wave)
    {
        if (currWave < waveSettings.waves.Length-1)
        {
            currWave++;
        }
        else
        {
            wavesHaveCompleted = true;
            onWavesCompleted?.Invoke();
        }
    }

}
}