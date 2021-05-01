using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BioTower.UI
{
public class GameCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private void Awake()
    {
        gameOverPanel.gameObject.SetActive(false);
    }

    public void OnPressRestart()
    {
        Debug.Log("Retry");
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
