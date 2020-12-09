using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

namespace BioTower
{
[CreateAssetMenu(fileName="WaveSettings", menuName="WaveSettings")]
public class LevelSettings : ScriptableObject
{
   [ReorderableList] public Wave[] waves;
}


[Serializable]
public class Wave
{
    public WaveMode state;
    public float startDelay;
    public int numEnemiesPerWave;
    public int spawnInterval;

    [HideInInspector] public float timeStarted;
    [HideInInspector] public float lastSpawn;
    [HideInInspector] public int numSpawns;

    // Not supported in naughty attributes yet
    //[ShowNativeProperty] public float waveDuration => (float) (startDelay + (numEnemiesPerWave * spawnInterval));

    public void Init()
    {
        state = WaveMode.NOT_STARTED;
        lastSpawn = 0;
        timeStarted = 0;
        numSpawns = 0;
    }


}
}
