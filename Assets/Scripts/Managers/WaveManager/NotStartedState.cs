using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    public class NotStartedState : WaveState
    {
        [SerializeField] private float delay = 2;
        private bool isReadyToGotoDelayState;
        public override void Init()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                LeanTween.delayedCall(gameObject, delay, () =>
                {
                    isReadyToGotoDelayState = true;
                    waveManager.currWave.timeStarted = Time.time;
                });

                EventManager.Wave.onWaveStateInit?.Invoke(waveState);
            }
        }

        public override WaveMode OnUpdate(WaveMode waveState)
        {
            Init();

            if (isReadyToGotoDelayState)
            {
                isReadyToGotoDelayState = false;
                waveState = WaveMode.DELAY;
            }

            return waveState;
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
