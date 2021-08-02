using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BioTower.UI
{
public class LevelSelectMenu : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform levelButtonContainer;
    private LevelSelectButton[] levelSelectButtons;
    
    private void Start()
    {
        levelSelectButtons = levelButtonContainer.GetComponentsInChildren<LevelSelectButton>();
        SetupButtons();
    }

    private void SetupButtons()
    {
        var gameData = GameManager.Instance.saveManager.Load();
        int currLevel = gameData.currLevel;

        for (int i=0; i<levelSelectButtons.Length; i++)
        {
            var btn = levelSelectButtons[i];
            if (i <= 0)
                btn.Unlock();
            else
                btn.Lock();
        }
    }
}
}