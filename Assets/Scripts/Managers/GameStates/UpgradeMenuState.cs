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
            var upgradePanelGo = controller.upgradePanel.panel.gameObject;
            upgradePanelGo.SetActive(true);

            float initLocalPosY = upgradePanelGo.transform.localPosition.y;
            upgradePanelGo.transform.localPosition = new Vector3(0, -500, 0);
            LeanTween.moveLocalY(upgradePanelGo, initLocalPosY, 0.25f).setEaseOutBack();

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