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

            if (isWin)
            {
                Display(goofyplantsWin);
                Hide(goofyplantsLose);
            }
            else
            {
                Display(goofyplantsLose);
                Hide(goofyplantsWin);
            }

            upgradeButtonGlow.StartGlowing();
        }

        public void OnPressRestart()
        {
            upgradeButtonGlow.StopGlowing();
            LevelSelectMenu.levelUnlocked = -1;
            Util.ReloadLevel();
        }

        public void Display(Image image)
        {
            image.gameObject.SetActive(true);
            var targetPos = image.transform.localPosition;
            image.transform.localPosition = targetPos + Vector3.down * 200;
            LeanTween.moveLocalY(image.gameObject, targetPos.y, 0.5f)
            .setEaseOutCubic()
            .setIgnoreTimeScale(true);

            LeanTween.scaleY(image.gameObject, 1.1f, 0.25f)
            .setLoopPingPong(1)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                LeanTween.scaleY(image.gameObject, 1.02f, 2.0f)
                .setIgnoreTimeScale(true)
                .setLoopPingPong(-1);
            });
        }

        public void Hide(Image image)
        {
            image.gameObject.SetActive(false);
        }

        public void OnPressUpgrade()
        {
            upgradeButtonGlow.StopGlowing();
            EventManager.UI.onPressUpgradeButton?.Invoke();
            EventManager.UI.onTapButton?.Invoke(true);

            var gameData = Util.saveManager.Load();
            var chosenUpgrades = gameData.chosenUpgrades;
            var numUpgrades = chosenUpgrades.Count;
            var currentLevel = (int)LevelInfo.current.levelType;

            //Debug.Log("numUpgrades: " + numUpgrades + ". Current Level: " + currentLevel);

            if (numUpgrades != currentLevel)
                LevelSelectMenu.levelUnlocked = currentLevel;
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
