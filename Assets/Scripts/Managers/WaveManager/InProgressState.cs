﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class InProgressState : WaveState
{
    private bool isWaveCancelled;

    public override void Init()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            GameManager.Instance.bootController.wavePanel.DisplayWaveTitle(waveManager.currWave);
            EventManager.Game.onWaveStateInit?.Invoke(waveState);
            Debug.Log("In Progress wave state init");
        }
    }

    public override WaveMode OnUpdate(Wave wave)
    {
        Init();

        if (Time.time > wave.lastSpawn + wave.spawnInterval || wave.numSpawns == 0)
        {
            waveManager.SpawnEnemy(wave.minMaxSpeed);
            wave.lastSpawn = Time.time;
            wave.numSpawns++;
        }

        //if (Time.time > wave.timeStarted + wave.startDelay + wave.duration)
        if ((wave.numSpawns >= wave.numEnemiesPerWave && !wave.isEndless) || isWaveCancelled)
        {
            isWaveCancelled = false;
            return WaveMode.ENDED;   
        }
        else
        {
            return wave.state;
        }
    }

    private void OnGameOver(bool isWin)
    {
        isWaveCancelled = true;
    }

    private void OnWaveStateInit(WaveMode waveState)
    {
        if (this.waveState != waveState)
            isInitialized = false;
    }

    private void OnEnable()
    {
        EventManager.Game.onWaveStateInit += OnWaveStateInit;
        EventManager.Game.onGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.Game.onWaveStateInit -= OnWaveStateInit;
        EventManager.Game.onGameOver -= OnGameOver;
    }
}
}
