using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class UpgradeMenuState : BootStateBase
{
     public override void Init(GameState gameState) 
    {
        if (!isInitialized)
        {
            isInitialized = true;
            controller.upgradePanel.panel.gameObject.SetActive(true);
            controller.upgradePanel.infoPanel.gameObject.SetActive(false);
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

    private void OnPressUpgradeButton()
    {
        controller.gameState = GameState.UPGRADE_MENU;
    }

    private void OnEnable()
    {
        EventManager.Game.onGameStateInit += OnGameStateInit;
        EventManager.UI.onPressUpgradeButton += OnPressUpgradeButton;
    }

    private void OnDisable()
    {
        EventManager.Game.onGameStateInit -= OnGameStateInit;
        EventManager.UI.onPressUpgradeButton -= OnPressUpgradeButton;
    }
}
}