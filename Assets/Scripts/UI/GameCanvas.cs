using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using BioTower.SaveData;

namespace BioTower.UI
{
public class GameCanvas : MonoBehaviour
{
    public Canvas canvas;
    public CanvasGroup canvasGroup;
    public CanvasGroup gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private void Awake()
    {
        gameOverPanel.gameObject.SetActive(false);
    }

    public void OnPressRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnPressUpgrade()
    {
        EventManager.UI.onPressUpgradeButton?.Invoke();
        EventManager.UI.onTapButton?.Invoke();
    }

    private void OnGameOver(bool isWin)
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverText.text = isWin ? "YOU WIN!" : "GAME OVER";
        
        // Unlocks next level upon win
        if (isWin)
        {
            var gameData = GameManager.Instance.saveManager.Load();
            int currLevelIndex = LevelInfo.current.levelIndex;
            gameData.currLevel = currLevelIndex++; 
        }
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
