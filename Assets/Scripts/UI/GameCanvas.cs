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
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private void Awake()
    {
        gameOverPanel.gameObject.SetActive(false);
    }

    public void OnPressRestart()
    {
        //Debug.Log("Retry");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
