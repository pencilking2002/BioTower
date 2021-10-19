using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BioTower.Units;

namespace BioTower
{
public class MissionController : MonoBehaviour
{
    [SerializeField] private GameObject missionPanel;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI missionDirectiveText;
    [SerializeField] private Color importantColor;
    private void Awake()
    {
        missionText.text = "";
        missionDirectiveText.text = "";
    }

    private void DisplayMissionText(string inputMissionText, string inputMissionDirectiveText=null)
    {
        var currScale = missionPanel.transform.localScale;
        missionPanel.transform.localScale = Vector3.zero;
        LeanTween.scale(missionPanel.gameObject, currScale, 0.5f).setEaseOutElastic();
        missionText.text = inputMissionText;

        if (!string.IsNullOrEmpty(inputMissionDirectiveText))
        {
            LeanTween.delayedCall(gameObject, 1.0f, () => {
                var cg = missionDirectiveText.GetComponent<CanvasGroup>();
                cg.alpha = 0;
                LeanTween.alphaCanvas(cg, 1.0f, 1.0f);
                missionDirectiveText.text = inputMissionDirectiveText;
            });
        }
    }

    private void OnLevelStarted(LevelType levelType)
    {
        if (!LevelInfo.current.IsFirstLevel())
        {
            LeanTween.delayedCall(gameObject, 1.0f, () => {
                var col = ColorUtility.ToHtmlStringRGB(importantColor);
                string missionText = $"Survive {Util.waveManager.waveSettings.waves.Length} waves";
                string directiveText = $"Current Wave: <color=#{col}>{Util.waveManager.currWave+1}</color>";
                DisplayMissionText(missionText, directiveText);
            });
        }
    }

    private void OnTutorialEnd(TutorialData data)
    {
        var levelInfo = LevelInfo.current;
        if (!levelInfo.IsFirstLevel())
            return;

        if (levelInfo.winCondition == WinCondition.KILL_ENEMIES)
        {
            LeanTween.delayedCall(gameObject, 1.0f, () => {
                var col = ColorUtility.ToHtmlStringRGB(importantColor);
                string missionText = $"Defeat {levelInfo.numEnemiesToDestroy} enemies";
                string directiveText = $"Enemies Defeated: <color=#{col}>0</color>";
                DisplayMissionText(missionText, directiveText);
            });
        }
    }

    private void OnUnitDestroyed(Unit unit)
    {
        if (unit.unitType != UnitType.BASIC_ENEMY)
            return;
            
        if (LevelInfo.current.IsFirstLevel())
        {
            var scale = Vector3.one;
            LeanTween.scale(missionDirectiveText.gameObject, scale * 1.2f, 0.2f).setLoopPingPong(1);
            missionDirectiveText.text = $"Enemies Defeated: <color=green>{LevelInfo.current.numEnemiesDestroyed}</color>";
        }
    }

    private void OnEnable()
    {
        EventManager.Game.onLevelStart += OnLevelStarted;
        EventManager.Tutorials.onTutorialEnd += OnTutorialEnd;
        EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
    }

    private void OnDisable()
    {
        EventManager.Game.onLevelStart -= OnLevelStarted;
        EventManager.Tutorials.onTutorialEnd -= OnTutorialEnd;        
        EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
    }
}
}