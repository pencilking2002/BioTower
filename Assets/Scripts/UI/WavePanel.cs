using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BioTower.UI
{
public class WavePanel : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private TextMeshProUGUI waveTitle;
    [SerializeField] private float displayDuration = 2.0f;
    private CanvasGroup waveTitleCG;

    private void Awake()
    {
        waveTitleCG = waveTitle.GetComponent<CanvasGroup>();
    }

    public void DisplayWaveTitle(int waveIndex)
    {
        waveTitle.text = $"WAVE {waveIndex+1}";
        panel.gameObject.SetActive(true);
        waveTitle.gameObject.SetActive(true);
        waveTitleCG.alpha = 0;

        var seq = LeanTween.sequence();
        seq.append(LeanTween.alphaCanvas(waveTitleCG, 1, 1));
        seq.append(displayDuration);
        seq.append(LeanTween.alphaCanvas(waveTitleCG, 0, 1.0f));
        seq.append(() => { 

            if (!GameManager.Instance.gameStates.IsGameState())
                return;
                
            panel.gameObject.SetActive(false);
            waveTitleCG.gameObject.SetActive(false); 
        });
    }
}
}