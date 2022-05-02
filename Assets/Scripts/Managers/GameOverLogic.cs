using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BioTower.Units;
using BioTower.SaveData;

namespace BioTower
{
    public enum WinCondition
    {
        SURVIVE_WAVES,
        KILL_ENEMIES
    }

    public enum LoseCondition
    {
        NONE,
        BASE_DESTROYED
    }

    public class GameOverLogic : MonoBehaviour
    {
        private void Awake()
        {

        }

        private void OnBaseDestroyed()
        {
            if (LevelInfo.current.loseCondition == LoseCondition.BASE_DESTROYED)
            {
                EventManager.Game.onGameOver?.Invoke(false, 0);
            }
        }

        private void OnUnitDestroyed(Unit unit)
        {
            var levelInfo = LevelInfo.current;
            if (levelInfo.winCondition == WinCondition.SURVIVE_WAVES)
            {
                if (unit.IsEnemy() &&
                    Util.unitManager.GetEnemyCount() <= 1 &&
                    Util.waveManager.wavesHaveCompleted)
                {

                    EventManager.Game.onGameOver?.Invoke(true, 2.0f);
                    Debug.Log("Game over. Num enemies left: " + Util.unitManager.GetEnemyCount());
                }
            }

            else if (levelInfo.winCondition == WinCondition.KILL_ENEMIES)
            {
                levelInfo.numEnemiesDestroyed++;
                if (levelInfo.numEnemiesDestroyed >= levelInfo.numEnemiesToDestroy)
                {
                    EventManager.Game.onGameOver?.Invoke(true, 0);
                }
            }
        }

        // private void OnWavesCompleted()
        // {
        //     if (LevelInfo.current.winCondition == WinCondition.SURVIVE_WAVES)
        //     {
        //         EventManager.Game.onGameOver?.Invoke(true);
        //     }
        // }

        private void OnGameStateInit(GameState gameState)
        {
            if (gameState == GameState.GAME_OVER_WIN || gameState == GameState.GAME_OVER_LOSE)
                Time.timeScale = 0;
            else if (gameState == GameState.GAME || gameState == GameState.LEVEL_SELECT)
                Time.timeScale = 1;
        }

        private void OnEnable()
        {
            EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
            EventManager.Game.onGameStateInit += OnGameStateInit;
            EventManager.Structures.onBaseDestroyed += OnBaseDestroyed;
            //EventManager.Game.onWavesCompleted += OnWavesCompleted;
            EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
            //EventManager.Game.onSpendCurrency += OnSpendCurrency;
        }

        private void OnDisable()
        {
            EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
            EventManager.Game.onGameStateInit -= OnGameStateInit;
            EventManager.Structures.onBaseDestroyed -= OnBaseDestroyed;
            //EventManager.Game.onWavesCompleted -= OnWavesCompleted;
            EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
            //EventManager.Game.onSpendCurrency -= OnSpendCurrency;
        }

    }
}