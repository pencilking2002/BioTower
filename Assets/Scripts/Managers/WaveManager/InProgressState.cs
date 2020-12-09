using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class InProgressState : WaveState
{
    public override WaveMode OnUpdate(Wave wave)
    {
        if (Time.time > wave.lastSpawn + wave.spawnInterval)
        {
            waveManager.SpawnEnemy();
            wave.lastSpawn = Time.time;
        }

        if (Time.time > wave.timeStarted + wave.startDelay + wave.duration)
        {
            return WaveMode.ENDED;   
        }
        else
        {
            return wave.state;
        }
    }
}
}
