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
    [SerializeField] private Color importantColor;
    private string htmlColor;
    private Vector3 initPos;
    private bool isDisplayed;

    private void Awake()
    {
        missionText.text = "";
        initPos = missionPanel.transform.position;
        missionPanel.transform.position = initPos + new Vector3(0,120,0);
        htmlColor = ColorUtility.ToHtmlStringRGB(importantColor);
    }
    
    private void OnLevelStarted(LevelType levelType)
    {
        if (!LevelInfo.current.IsFirstLevel())
        {
            // string missionText = $"Wave <color=#{htmlColor}>1/{Util.waveManager.waveSettings.waves.Length}</color>";
            // SlideInPanel(missionText, 2.0f);
        }
        else
        {
            missionPanel.SetActive(false);
        }
    }

    private void SlideInPanel(string inputMissionText, float delay)
    {
        if (isDisplayed)
            return;

        //Debug.Log("Slide in");
        isDisplayed = true;
        var startPos = initPos;
        startPos.y += 120;
        missionPanel.transform.position = startPos;
        LeanTween.delayedCall(gameObject, delay, () => {
            missionPanel.SetActive(true);
            LeanTween.move(missionPanel.gameObject, initPos, 0.5f).setEaseOutQuint();
            missionText.text = inputMissionText;
        });
    }

    private void OnTutorialEnd(TutorialData data)
    {
        var levelInfo = LevelInfo.current;
        // if (!levelInfo.IsFirstLevel())
        //     return;

        if (levelInfo.winCondition == WinCondition.KILL_ENEMIES)
        {
            string text = $"Enemies defeated <color=#{htmlColor}>0/{levelInfo.numEnemiesToDestroy}</color>";
            SlideInPanel(text, 1.0f);
        }
    }

    private void OnUnitDestroyed(Unit unit)
    {
        if (unit.unitType != UnitType.BASIC_ENEMY)
            return;
            
        if (LevelInfo.current.IsFirstLevel())
        {
            var scale = Vector3.one;
            LeanTween.scale(missionPanel.gameObject, scale * 1.2f, 0.2f).setLoopPingPong(1);
            string text = $"Enemies defeated <color=#{htmlColor}>{LevelInfo.current.numEnemiesDestroyed}/{LevelInfo.current.numEnemiesToDestroy}</color>";
            missionText.text = text;
        }
    }

    private void OnWaveStateInit(WaveMode waveState)
    {
        if (waveState != WaveMode.IN_PROGRESS)
            return;
        
        //var scale = Vector3.one;
        //LeanTween.scale(missionPanel.gameObject, scale * 1.2f, 0.2f).setLoopPingPong(1);
        string text = $"Wave <color=#{htmlColor}>{Util.waveManager.currWaveIndex+1}/{Util.waveManager.waveSettings.waves.Length}</color>";
        missionText.text = text;
        SlideInPanel(text, 1.5f);

    }

    private void OnEnable()
    {
        EventManager.Game.onLevelStart += OnLevelStarted;
        EventManager.Tutorials.onTutorialEnd += OnTutorialEnd;
        EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
        EventManager.Game.onWaveStateInit += OnWaveStateInit;
    }

    private void OnDisable()
    {
        EventManager.Game.onLevelStart -= OnLevelStarted;
        EventManager.Tutorials.onTutorialEnd -= OnTutorialEnd;        
        EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
        EventManager.Game.onWaveStateInit -= OnWaveStateInit;
    }
}
}