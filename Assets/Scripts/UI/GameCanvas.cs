using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        Debug.Log("Upgrade");
        EventManager.UI.onPressUpgradeButton?.Invoke();
    }

    private void OnGameOver(bool isWin)
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverText.text = isWin ? "YOU WIN!" : "GAME OVER";
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
