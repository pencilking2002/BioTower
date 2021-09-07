using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BioTower
{
public class LevelSelectMenuState : BootStateBase
{
    public override void Init(GameState gameState) 
    {
        if (!isInitialized)
        {
            isInitialized = true;
            BootController.isBootLoaded = true;
            controller.gameCanvas.canvas.enabled = false;
            controller.gameCanvas.canvasGroup.alpha = 0;
            controller.gameCanvas.gameOverPanel.gameObject.SetActive(false);
            controller.upgradePanel.panel.gameObject.SetActive(false);
            controller.levelSelectMenu.canvas.enabled = true;
            controller.startMenuCanvas.menuPanel.gameObject.SetActive(false);

            EventManager.Game.onGameStateInit?.Invoke(gameState);
        }
    }

    public override GameState OnUpdate(GameState gameState)
    {
        Init(gameState);
        return gameState;
    }

    public override void OnGameStateInit(GameState gameState)
    {
        if (gameState != this.gameState)
            isInitialized = false;
    }

    private void onTapLevelSelectButton()
    {
        controller.gameState = GameState.GAME;
    }

    private void OnEnable()
    {
        EventManager.Game.onGameStateInit += OnGameStateInit;
        EventManager.UI.onTapLevelSelectButton += onTapLevelSelectButton;
    }

    private void OnDisable()
    {
        EventManager.Game.onGameStateInit -= OnGameStateInit;
        EventManager.UI.onTapLevelSelectButton -= onTapLevelSelectButton;
    }


}   
}