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
    public int waveIndex;
    public float startDelay;
    public int numEnemiesPerWave;
    public int spawnInterval;
    [MinMaxSlider(0, 1)]
    public Vector2 minMaxSpeed = new Vector2(0.4f, 0.7f);

    [HideInInspector] public float timeStarted;
    [HideInInspector] public float lastSpawn;
    [HideInInspector] public int numSpawns;

    // Not supported in naughty attributes yet
    public float waveDuration => (float) (startDelay + (numEnemiesPerWave * spawnInterval));

    public void Init(int waveIndex)
    {
        state = WaveMode.NOT_STARTED;
        lastSpawn = 0;
        timeStarted = 0;
        numSpawns = 0;
        this.waveIndex = waveIndex; 
    }


}
}
