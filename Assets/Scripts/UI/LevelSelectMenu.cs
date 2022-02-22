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

        public static int levelUnlocked = -1;

        private void Start()
        {
            levelSelectButtons = levelButtonContainer.GetComponentsInChildren<LevelSelectButton>();
            SetupButtons();

            if (levelUnlocked != -1)
            {
                Debug.Log("New level unlocked: " + levelUnlocked);
            }
        }

        public LevelSelectButton[] GetButtons()
        {
            return levelSelectButtons;
        }

        private void SetupButtons()
        {
            var gameData = GameManager.Instance.saveManager.Load();
            int currLevel = gameData.settings.currLevel;

            for (int i = 0; i < levelSelectButtons.Length; i++)
            {
                var btn = levelSelectButtons[i];
                if (i < currLevel)
                {
                    if (i == levelUnlocked)
                        btn.Unlock(1);
                    else
                        btn.Unlock();
                }
                else
                    btn.Lock();
            }
        }
    }
}