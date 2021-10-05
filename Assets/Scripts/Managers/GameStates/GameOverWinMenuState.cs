using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.UI;

namespace BioTower
{
public class GameOverWinMenuState : BootStateBase
{
     public override void Init(GameState gameState) 
    {
        if (!isInitialized)
        {
            isInitialized = true;
            controller.gameplayUI.gameUIPanel.gameObject.SetActive(false);
            
            var saveData = GameManager.Instance.saveManager.Load();
            saveData.settings = Util.upgradeSettings;
            GameManager.Instance.saveManager.Save(saveData);

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