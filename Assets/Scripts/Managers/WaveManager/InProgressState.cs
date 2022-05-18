using UnityEngine;
using BioTower.Units;

namespace BioTower
{
    public class InProgressState : WaveState
    {
        public override void Init()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                var wave = waveManager.currWave;
                wave.lastSpawnIntervalRange = wave.CreateSpawnIntervalFromRange();
                EventManager.Wave.onWaveStateInit?.Invoke(waveState);

                var spawnPoint = GameManager.Instance.GetWaypointManager().GetSpawnPoint(waveManager.currWave.waypointIndex);
                EventManager.Wave.onStopWaveWarning?.Invoke(spawnPoint);
            }
        }

        public override WaveMode OnUpdate(WaveMode waveState)
        {
            Init();
            var wave = waveManager.currWave;
            if ((Time.time > wave.lastSpawn + wave.lastSpawnIntervalRange ||
                wave.numSpawns == 0) && wave.numSpawns < wave.numEnemiesPerWave)
            {
                var numEnemiesToSpawn = GetNumSpawns(wave);
                wave.lastSpawnIntervalRange = wave.CreateSpawnIntervalFromRange();

                for (int i = 0; i < numEnemiesToSpawn; i++)
                    waveManager.SpawnEnemy(wave.enemyType);

                wave.lastSpawn = Time.time;
                wave.numSpawns = wave.numSpawns + numEnemiesToSpawn;
            }

            //if ((wave.numSpawns >= wave.numEnemiesPerWave && !wave.isEndless))
            if (wave.allEnemiesDead && !wave.isEndless)
            {
                waveState = WaveMode.ENDED;
                //Debug.Log($"wave ended.");
            }
            return waveState;
        }

        private int GetNumSpawns(Wave wave)
        {
            var numEnemiesToSpawns = 1;
            if (wave.enableMultipleSpawnsAtOnce)
            {
                numEnemiesToSpawns = UnityEngine.Random.Range(1, wave.maxNumSpawnsAtOnce + 1);

                if (numEnemiesToSpawns + wave.numSpawns > wave.numEnemiesPerWave)
                    numEnemiesToSpawns = wave.numEnemiesPerWave - wave.numSpawns;
            }

            return numEnemiesToSpawns;
        }

        private void OnGameOver(bool isWin, float delay)
        {
            waveManager.SetEndedState();
        }

        private void OnWaveStateInit(WaveMode waveState)
        {
            if (this.waveState != waveState)
                isInitialized = false;
        }

        private void OnUnitDestroyed(Unit unit)
        {
            if (!unit.IsEnemy())
                return;

            var enemyUnit = (EnemyUnit)unit;
            if (enemyUnit.waveIndex == waveManager.currWaveIndex)
                waveManager.currWave.IncrementNumDead();
        }

        private void OnEnable()
        {
            EventManager.Wave.onWaveStateInit += OnWaveStateInit;
            EventManager.Game.onGameOver += OnGameOver;
            EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
        }

        private void OnDisable()
        {
            EventManager.Wave.onWaveStateInit -= OnWaveStateInit;
            EventManager.Game.onGameOver -= OnGameOver;
            EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;

        }
    }
}
