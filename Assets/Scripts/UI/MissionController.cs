using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BioTower
{
public class MissionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionText;

    private void Awake()
    {
        missionText.text = "";
    }

    private void DisplayMissionText(string inputText)
    {
        var currScale = missionText.transform.localScale;
        missionText.transform.localScale = Vector3.zero;
        LeanTween.scale(missionText.gameObject, currScale, 0.5f).setEaseOutElastic();
        missionText.text = inputText;
    }

    private void OnLevelStarted(LevelType levelType)
    {
    }

    private void OnTutorialEnd(TutorialData data)
    {
        var levelInfo = LevelInfo.current;
        if (!levelInfo.IsFirstLevel())
            return;

        if (levelInfo.winCondition == WinCondition.KILL_ENEMIES)
        {
            LeanTween.delayedCall(gameObject, 1.0f, () => {
                string text = "Defeat 3 enemies";
                DisplayMissionText(text);
            });
        }
    }

    private void OnEnable()
    {
        EventManager.Game.onLevelStart += OnLevelStarted;
        EventManager.Tutorials.onTutorialEnd += OnTutorialEnd;
    }

    private void OnDisable()
    {
        EventManager.Game.onLevelStart -= OnLevelStarted;
        EventManager.Tutorials.onTutorialEnd -= OnTutorialEnd;
    }
}
}