using UnityEngine;

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
                EventManager.Game.onWaveStateInit?.Invoke(waveState);
            }
        }

        public override WaveMode OnUpdate(WaveMode waveState)
        {
            Init();
            var wave = waveManager.currWave;
            if (Time.time > wave.lastSpawn + wave.lastSpawnIntervalRange || wave.numSpawns == 0)
            {
                wave.lastSpawnIntervalRange = wave.CreateSpawnIntervalFromRange();
                waveManager.SpawnEnemy(wave.enemyType);
                wave.lastSpawn = Time.time;
                wave.numSpawns++;
            }

            if ((wave.numSpawns >= wave.numEnemiesPerWave && !wave.isEndless))
            {
                waveState = WaveMode.ENDED;
            }
            return waveState;
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

        private void OnEnable()
        {
            EventManager.Game.onWaveStateInit += OnWaveStateInit;
            EventManager.Game.onGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            EventManager.Game.onWaveStateInit -= OnWaveStateInit;
            EventManager.Game.onGameOver -= OnGameOver;
        }
    }
}
