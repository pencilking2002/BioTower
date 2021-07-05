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