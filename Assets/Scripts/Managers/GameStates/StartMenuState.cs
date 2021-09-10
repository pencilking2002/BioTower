using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BioTower
{
public class StartMenuState : BootStateBase
{
    public override void Init(GameState gameState) 
    {
        if (!isInitialized)
        {
            isInitialized = true;
            controller.gameCanvas.canvas.enabled = false;
            controller.gameCanvas.canvasGroup.alpha = 0;
            controller.gameCanvas.gameOverPanel.gameObject.SetActive(false);
            controller.upgradePanel.Hide();
            controller.levelSelectMenu.canvas.enabled = false;

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

    private void OnTapStartMenu()
    {
        if (isInitialized)
        {
            controller.gameState = GameState.LEVEL_SELECT;
            //controller.LoadFirstScene();
        }
    }

    private void OnEnable()
    {
        EventManager.Input.onTapStartMenu += OnTapStartMenu;
        EventManager.Game.onGameStateInit += OnGameStateInit;
    }

    private void OnDisable()
    {
        EventManager.Input.onTapStartMenu -= OnTapStartMenu;
        EventManager.Game.onGameStateInit -= OnGameStateInit;
    }


}   
}