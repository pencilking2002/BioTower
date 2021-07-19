using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class DelayState : WaveState
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
        
        if (Time.time > wave.timeStarted + wave.startDelay)
        {
            return WaveMode.IN_PROGRESS;
        }
        else
        {
            return wave.state;
        }
    }

    private void OnWaveStateInit(WaveMode waveState)
    {
        if (this.waveState != waveState)
            isInitialized = false;
    }

    private void OnEnable()
    {
        EventManager.Game.onWaveStateInit += OnWaveStateInit;
    }

    private void OnDisable()
    {
        EventManager.Game.onWaveStateInit -= OnWaveStateInit;
    }
}
}