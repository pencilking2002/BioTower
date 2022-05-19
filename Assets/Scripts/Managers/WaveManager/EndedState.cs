using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    public class EndedState : WaveState
    {
        public override void Init()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                var numWaves = waveManager.waveSettings.waves.Length;
                EventManager.Wave.onWaveStateInit?.Invoke(waveState);
            }
        }

        public override WaveMode OnUpdate(WaveMode waveState)
        {
            Init();
            if (!waveManager.wavesHaveCompleted)
            {
                var wave = waveManager.currWave;
                if (waveManager.currWaveIndex < waveManager.waveSettings.waves.Length - 1)
                {
                    ++waveManager.currWaveIndex;
                    waveState = WaveMode.NOT_STARTED;
                }
                else
                {
                    //waveManager.wavesHaveCompleted = true;
                    EventManager.Wave.onWavesCompleted?.Invoke();
                }
            }

            return waveState;
        }

        private void OnWaveStateInit(WaveMode waveState)
        {
            if (this.waveState != waveState)
                isInitialized = false;
        }

        private void OnLevelStart(LevelType levelType)
        {
            if (levelType != LevelType.NONE)
                waveManager.SetNotStartedState();
        }

        private void OnEnable()
        {
            EventManager.Wave.onWaveStateInit += OnWaveStateInit;
            EventManager.Game.onLevelStart += OnLevelStart;
        }

        private void OnDisable()
        {
            EventManager.Wave.onWaveStateInit -= OnWaveStateInit;
            EventManager.Game.onLevelStart -= OnLevelStart;
        }
    }
}
