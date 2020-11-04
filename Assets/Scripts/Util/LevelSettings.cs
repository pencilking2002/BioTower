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
    public int numEnemiesPerWave;
}
}
