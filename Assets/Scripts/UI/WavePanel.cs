﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BioTower.UI
{
    public class WavePanel : MonoBehaviour
    {
        public CanvasGroup panel;
        [SerializeField] private TextMeshProUGUI waveTitle;
        [SerializeField] private float displayDuration = 2.0f;
        private CanvasGroup waveTitleCG;

        private void Awake()
        {
            waveTitleCG = waveTitle.GetComponent<CanvasGroup>();
            panel.gameObject.SetActive(false);
        }

        public void DisplayWaveTitle(string message)
        {
            if (LevelInfo.current.IsFirstLevel())
                return;

            //     waveTitle.text = $"WAVE {waveIndex + 1} STARTED!";
            // else
            waveTitle.text = message;

            panel.gameObject.SetActive(true);
            panel.alpha = 1;

            var seq = LeanTween.sequence();
            panel.transform.localScale = Vector3.zero;
            seq.append(LeanTween.scale(panel.gameObject, Vector3.one, 0.25f).setEaseOutBack());
            seq.append(displayDuration);
            seq.append(LeanTween.alphaCanvas(panel, 0, 0.5f));

            seq.append(() =>
            {
                if (!GameManager.Instance.gameStates.IsGameState())
                    return;

                panel.gameObject.SetActive(false);
            });
        }

        private void OnWaveStateInit(WaveMode waveState)
        {
            if (waveState == WaveMode.IN_PROGRESS)
            {
                var message = $"WAVE {Util.waveManager.currWaveIndex + 1} STARTED";
                DisplayWaveTitle(message);
            }
            else if (waveState == WaveMode.ENDED)
            {
                var message = $"WAVE {Util.waveManager.currWaveIndex + 1} DEFEATED!";
                DisplayWaveTitle(message);
            }
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