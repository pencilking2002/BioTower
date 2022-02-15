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
        public Image background;
        public TextMeshProUGUI gameOverText;
        public RectTransform upgradeButton;
        public RectTransform restartButton;
        public Image goofyplantsWin;
        public Image goofyplantsLose;
        public AnimatedGlow upgradeButtonGlow;

        private void Awake()
        {
            panel.gameObject.SetActive(false);
        }

        private void OnGameOver(bool isWin)
        {
            panel.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
            gameOverText.text = isWin ? "YOU WIN!" : "GAME OVER";

            upgradeButton.gameObject.SetActive(isWin);
            restartButton.gameObject.SetActive(!isWin);

            goofyplantsWin.gameObject.SetActive(isWin);
            goofyplantsLose.gameObject.SetActive(!isWin);

            upgradeButtonGlow.StartGlowing();
        }

        public void OnPressRestart()
        {
            upgradeButtonGlow.StopGlowing();
            Util.ReloadLevel();
        }

        public void OnPressUpgrade()
        {
            upgradeButtonGlow.StopGlowing();
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
