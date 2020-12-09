using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class EndedState : WaveState
{
    public override WaveMode OnUpdate(Wave wave)
    {
        if (waveManager.currWave < waveManager.waveSettings.waves.Length-1)
        {
            waveManager.currWave++;
        }
        else
        {
            waveManager.wavesHaveCompleted = true;
            onWavesCompleted?.Invoke();
        }

        return wave.state;
    }
}
}
