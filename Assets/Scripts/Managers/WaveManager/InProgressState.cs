using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class InProgressState : WaveState
{
    public override WaveMode OnUpdate(Wave wave)
    {
        if (Time.time > wave.lastSpawn + wave.spawnInterval || wave.numSpawns == 0)
        {
            waveManager.SpawnEnemy(wave.minMaxSpeed);
            wave.lastSpawn = Time.time;
            wave.numSpawns++;
        }

        //if (Time.time > wave.timeStarted + wave.startDelay + wave.duration)
        if (wave.numSpawns >= wave.numEnemiesPerWave)
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
