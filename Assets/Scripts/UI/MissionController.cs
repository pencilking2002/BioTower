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
        private int seconds;

        private void Awake()
        {
            missionText.text = "";
            initPos = missionPanel.transform.position;
            missionPanel.transform.position = initPos + new Vector3(0, 120, 0);
            htmlColor = ColorUtility.ToHtmlStringRGB(importantColor);
        }

        private void Update()
        {
            if (Util.waveManager.waveMode == WaveMode.DELAY)
            {
                float timeOfStart = Util.waveManager.currWave.timeStarted + Util.waveManager.currWave.startDelay;
                float timeRemaining = timeOfStart - Time.time;
                int newSeconds = (int)Mathf.Ceil(timeRemaining);

                missionText.text = "WAVE starting in " + newSeconds;

                if (seconds != newSeconds)
                    EventManager.Game.onWaveCountdownTick?.Invoke(newSeconds);

                seconds = newSeconds;
            }
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

        private void DisplayPanel(string inputMissionText, float delay = 0)
        {
            missionText.text = inputMissionText;

            if (!isDisplayed)
            {
                isDisplayed = true;
                var startPos = initPos;
                startPos.y += 120;
                missionPanel.transform.position = startPos;
                LeanTween.delayedCall(gameObject, delay, () =>
                {
                    missionPanel.SetActive(true);
                    LeanTween.move(missionPanel.gameObject, initPos, 0.5f).setEaseOutQuint();
                });
            }
        }

        private void OnTutorialEnd(TutorialData data)
        {
            var levelInfo = LevelInfo.current;
            // if (!levelInfo.IsFirstLevel())
            //     return;

            if (levelInfo.winCondition == WinCondition.KILL_ENEMIES)
            {
                string text = $"Enemies defeated <color=#{htmlColor}>0/{levelInfo.numEnemiesToDestroy}</color>";
                DisplayPanel(text, 1.0f);
            }
        }

        private void OnUnitDestroyed(Unit unit)
        {
            if (!unit.IsEnemy())
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
            if (waveState == WaveMode.NOT_STARTED)
            {
                string message = Util.waveManager.currWaveIndex == 0 ? "GET READY!" : "WAVE DEFEATED!";
                DisplayPanel(message);
            }

            else if (waveState == WaveMode.IN_PROGRESS)
            {
                string text = $"Wave <color=#{htmlColor}>{Util.waveManager.currWaveIndex + 1}/{Util.waveManager.waveSettings.waves.Length}</color>";
                missionText.text = text;
                DisplayPanel(text, 1.5f);
            }

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