﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.UI;

namespace BioTower
{
    public class GameOverLoseMenuState : BootStateBase
    {
        public override void Init(GameState gameState)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                controller.gameplayUI.panel.gameObject.SetActive(false);
                EventManager.Game.onGameStateInit?.Invoke(gameState);
                Util.saveManager.LoadAndSave();
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

        private void OnGameOver(bool isWin, float delay)
        {
            if (isInitialized)
                return;

            if (isWin)
                return;

            controller.gameState = GameState.GAME_OVER_LOSE;
        }

        private void OnEnable()
        {
            EventManager.Game.onGameStateInit += OnGameStateInit;
            EventManager.Game.onGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            EventManager.Game.onGameStateInit -= OnGameStateInit;
            EventManager.Game.onGameOver -= OnGameOver;
        }
    }
}