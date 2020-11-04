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

public enum WaveState
{
    NOT_STARTED,
    DELAY,
    IN_PROGRESS,
    ENDED
}

[Serializable]
public class Wave
{
    public WaveState state;
    public int numEnemiesPerWave;
    public int spawnInterval;
    public float duration;
    public float startDelay;
    [HideInInspector] public float timeStarted;
    [HideInInspector] public float lastSpawn;
    
    public void SetNotStarted() { state = WaveState.NOT_STARTED; }
    public void SetDelay() { state = WaveState.DELAY; }
    public void SetInProgress() { state = WaveState.IN_PROGRESS; }
    public void SetEnded() { state = WaveState.ENDED; }


}
}
