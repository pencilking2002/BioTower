using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    public class DelayState : WaveState
    {
        public override void Init()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                EventManager.Wave.onWaveStateInit?.Invoke(waveState);
                var spawnPoint = GameManager.Instance.GetWaypointManager().GetSpawnPoint(waveManager.currWave.waypointIndex);
                EventManager.Wave.onDisplayWaveWarning?.Invoke(spawnPoint);
            }
        }

        public override WaveMode OnUpdate(WaveMode waveState)
        {
            Init();

            var wave = waveManager.currWave;
            if (Time.time > wave.timeStarted + wave.startDelay)
            {
                //Debug.Log("Start in progress state");
                return WaveMode.IN_PROGRESS;
            }
            else
            {
                return waveState;
            }
        }

        private void OnWaveStateInit(WaveMode waveState)
        {
            if (this.waveState != waveState)
                isInitialized = false;
        }

        private void OnEnable()
        {
            EventManager.Wave.onWaveStateInit += OnWaveStateInit;
        }

        private void OnDisable()
        {
            EventManager.Wave.onWaveStateInit -= OnWaveStateInit;
        }
    }
}