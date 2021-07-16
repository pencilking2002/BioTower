using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BioTower
{
public class GameMenuState : BootStateBase
{
    public override void Init(GameState gameState) 
    {
        if (!isInitialized)
        {
            isInitialized = true;
            
            controller.levelSelectMenu.canvas.enabled = false; 

            var seq = LeanTween.sequence();
            seq.append(2.0f);
            seq.append(() => { 
                controller.gameCanvas.canvas.enabled = true;
                controller.gameCanvas.canvasGroup.alpha = 0;
                controller.gameCanvas.gameOverPanel.gameObject.SetActive(false);

                controller.gameplayUI.gameUIPanel.gameObject.SetActive(true);
                controller.upgradePanel.panel.gameObject.SetActive(false);
                //controller.towerMenu.towerPanel.gameObject.SetActive(true);
            });
            seq.append(LeanTween.alphaCanvas(controller.gameCanvas.canvasGroup, 1.0f, 0.5f));

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

    private void OnEnable()
    {
        EventManager.Game.onGameStateInit += OnGameStateInit;
    }

    private void OnDisable()
    {
        EventManager.Game.onGameStateInit -= OnGameStateInit;
    }


}   
}