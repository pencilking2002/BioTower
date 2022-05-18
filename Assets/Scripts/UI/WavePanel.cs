using System.Collections;
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

        public void DisplayWaveTitle(int waveIndex)
        {
            if (LevelInfo.current.IsFirstLevel())
                return;

            waveTitle.text = $"WAVE {waveIndex + 1} STARTED!";
            panel.gameObject.SetActive(true);
            panel.alpha = 1;

            var seq = LeanTween.sequence();
            //seq.append(LeanTween.alphaCanvas(panel, 1, 1));
            panel.transform.localScale = Vector3.zero;
            seq.append(LeanTween.scale(panel.gameObject, Vector3.one, 0.25f).setEaseOutQuart());
            seq.append(displayDuration);
            seq.append(LeanTween.alphaCanvas(panel, 0, 0.5f));

            //seq.append(LeanTween.alphaCanvas(panel, 0, 1.0f));
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
                DisplayWaveTitle(Util.waveManager.currWaveIndex);
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