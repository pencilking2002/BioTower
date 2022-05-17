using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using BioTower.Units;

namespace BioTower
{
    [CreateAssetMenu(fileName = "WaveSettings", menuName = "WaveSettings")]
    public class WaveSettings : ScriptableObject
    {
        public int startingEnergy = 100;
        public Wave[] waves;
    }


    [Serializable]
    public class Wave
    {
        public UnitType enemyType;
        public bool isEndless;
        [HideIf("isEndless")] public int numEnemiesPerWave;
        public float startDelay;
        public int waypointIndex;
        public Vector2 spawnIntervalRange = new Vector2(1, 1);
        [BoxGroup("Multiple Spawns")] public bool enableMultipleSpawnsAtOnce;
        [BoxGroup("Multiple Spawns")][ShowIf("enableMultipleSpawnsAtOnce")] public int maxNumSpawnsAtOnce;

        [HideInInspector] public float timeStarted;
        [HideInInspector] public float lastSpawn;
        [HideInInspector] public int numSpawns;
        [HideInInspector] public float lastSpawnIntervalRange;
        [HideInInspector] public bool allEnemiesDead;
        [HideInInspector] public int numDead;

        public float CreateSpawnIntervalFromRange()
        {
            return UnityEngine.Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
        }

        public void IncrementNumDead()
        {
            ++this.numDead;
            if (this.numDead >= this.numEnemiesPerWave)
                allEnemiesDead = true;
        }

        public void Init(int waveIndex)
        {
            lastSpawn = 0;
            timeStarted = 0;
            numSpawns = 0;
            numDead = 0;
            allEnemiesDead = false;
        }


    }
}
