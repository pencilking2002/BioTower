using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class EndedState : WaveState
{
    public override void Init()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            EventManager.Game.onWaveStateInit?.Invoke(waveState);
        }
    }

    public override WaveMode OnUpdate(Wave wave)
    {
        Init();

        if (waveManager.currWave < waveManager.waveSettings.waves.Length-1)
        {
            waveManager.currWave++;
        }
        else
        {
            waveManager.wavesHaveCompleted = true;
            EventManager.Game.onWavesCompleted?.Invoke();
        }

        return wave.state;
    }

    private void OnWaveStateInit(WaveMode waveState)
    {
        if (this.waveState != waveState)
            isInitialized = false;
    }

    private void OnLevelStart(LevelType levelType)
    {
        if (levelType != LevelType.NONE)
            waveManager.SetNotStartedState();
    }

    private void OnEnable()
    {
        EventManager.Game.onWaveStateInit += OnWaveStateInit;
        EventManager.Game.onLevelStart += OnLevelStart;
    }

    private void OnDisable()
    {
        EventManager.Game.onWaveStateInit -= OnWaveStateInit;
        EventManager.Game.onLevelStart -= OnLevelStart;
    }
}
}
