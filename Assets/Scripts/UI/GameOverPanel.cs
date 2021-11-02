using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BioTower.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        public CanvasGroup panel;
        public TextMeshProUGUI gameOverText;
        public Button upgradeButton;
        public Button restartButton;

        private void Awake()
        {
            panel.gameObject.SetActive(false);
        }

        private void OnGameOver(bool isWin)
        {
            panel.gameObject.SetActive(true);

            if (isWin)
            {
                gameOverText.text = "YOU WIN!";
                upgradeButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);
            }
            else
            {
                gameOverText.text = "GAME OVER";
                upgradeButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(true);
            }
        }

        public void OnPressRestart()
        {
            Util.ReloadLevel();
        }

        public void OnPressUpgrade()
        {
            EventManager.UI.onPressUpgradeButton?.Invoke();
            EventManager.UI.onTapButton?.Invoke(true);
        }

        private void OnEnable()
        {
            EventManager.Game.onGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            EventManager.Game.onGameOver -= OnGameOver;
        }

    }
}
