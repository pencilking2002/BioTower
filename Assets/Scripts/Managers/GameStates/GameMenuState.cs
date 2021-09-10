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
                controller.gameCanvas.canvasGroup.alpha = 1;
                controller.gameCanvas.gameOverPanel.gameObject.SetActive(false);

                controller.gameplayUI.gameUIPanel.alpha = 0;
                controller.gameplayUI.gameUIPanel.gameObject.SetActive(true);

                controller.upgradePanel.Hide();
                //controller.towerMenu.towerPanel.gameObject.SetActive(true);
            });

            seq.append(() => {
                // Don't display the menu at the beginning of the level if we're in the beginning of the level
                // and doing a tutorial
                var tutCanvas = GameManager.Instance.currTutCanvas;
                if (tutCanvas.hasTutorials)
                {
                    var requiredAction = tutCanvas.currTutorial.requiredAction;
                    if ((TutorialCanvas.tutorialInProgress && requiredAction != RequiredAction.TAP_ABA_TOWER_BUTTON))
                    {

                    }
                    else
                    {
                        LeanTween.alphaCanvas(controller.gameplayUI.gameUIPanel, 1.0f, 0.5f);
                    }
                }
                else
                {
                    LeanTween.alphaCanvas(controller.gameplayUI.gameUIPanel, 1.0f, 0.5f);
                }
                
            });

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

    private void OnTutorialStart(TutorialData tut)
    {
        if (tut.requiredAction == RequiredAction.TAP_ABA_TOWER_BUTTON)
        {
            LeanTween.alphaCanvas(controller.gameplayUI.gameUIPanel, 1.0f, 0.5f);
        }
    }

    private void OnGameOver(bool isWin)
    {
        if (!isInitialized)
            return;
            
        if (isWin)
            controller.gameState = GameState.GAME_OVER_WIN;
        else
            controller.gameState = GameState.GAME_OVER_LOSE;    
    }

    private void OnEnable()
    {
        EventManager.Game.onGameStateInit += OnGameStateInit;
        EventManager.Tutorials.onTutorialStart += OnTutorialStart;
        EventManager.Game.onGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventManager.Game.onGameStateInit -= OnGameStateInit;
        EventManager.Tutorials.onTutorialStart -= OnTutorialStart;
        EventManager.Game.onGameOver -= OnGameOver;
    }
}   
}