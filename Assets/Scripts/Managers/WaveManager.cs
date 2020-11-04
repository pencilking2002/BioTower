using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

namespace BioTower
{
public class WaveManager : MonoBehaviour
{
    [SerializeField] private LevelSettings waveSettings; 
    [SerializeField] private int currWave;
    [SerializeField] private int numEnemiesPerWave;

}
}