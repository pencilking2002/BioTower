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
            if (!GameManager.Instance.gameStates.IsGameState())
                return;

            if (LevelInfo.current.loseCondition == LoseCondition.BASE_DESTROYED)
            {
                EventManager.Game.onGameOver?.Invoke(false, 0);
            }
        }

        private void OnUnitDestroyed(Unit unit)
        {
            if (!GameManager.Instance.gameStates.IsGameState())
                return;

            if (!unit.IsEnemy())
                return;

            var levelInfo = LevelInfo.current;
            if (levelInfo.winCondition == WinCondition.SURVIVE_WAVES)
            {
                if (Util.unitManager.GetEnemyCount() <= 1 &&
                    Util.waveManager.wavesHaveCompleted)
                {

                    EventManager.Game.onGameOver?.Invoke(true, 2.0f);
                    Debug.Log("Game over. Num enemies left: " + Util.unitManager.GetEnemyCount());
                }
            }

            else if (levelInfo.winCondition == WinCondition.KILL_ENEMIES)
            {
                var enemyUnit = (EnemyUnit)unit;
                if (enemyUnit.baseReached)
                    return;

                Debug.Log("unit destroyed: " + unit.name);
                levelInfo.numEnemiesDestroyed++;
                if (levelInfo.numEnemiesDestroyed >= levelInfo.numEnemiesToDestroy)
                {
                    EventManager.Game.onGameOver?.Invoke(true, 1.0f);
                }
            }
        }

        private void OnGameStateInit(GameState gameState)
        {
            if (gameState == GameState.GAME_OVER_WIN || gameState == GameState.GAME_OVER_LOSE)
                Time.timeScale = 0;
            else if (gameState == GameState.GAME || gameState == GameState.LEVEL_SELECT)
                Time.timeScale = 1;
        }

        private void OnWavesHaveCompleted()
        {
            if (!GameManager.Instance.gameStates.IsGameState())
                return;

            var levelInfo = LevelInfo.current;
            if (levelInfo.winCondition == WinCondition.SURVIVE_WAVES)
            {
                EventManager.Game.onGameOver?.Invoke(true, 2.0f);
            }

        }

        private void OnEnable()
        {
            EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
            EventManager.Game.onGameStateInit += OnGameStateInit;
            EventManager.Structures.onBaseDestroyed += OnBaseDestroyed;
            EventManager.Wave.onWavesCompleted += OnWavesHaveCompleted;
        }

        private void OnDisable()
        {
            EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
            EventManager.Game.onGameStateInit -= OnGameStateInit;
            EventManager.Structures.onBaseDestroyed -= OnBaseDestroyed;
            EventManager.Wave.onWavesCompleted -= OnWavesHaveCompleted;
        }

    }
}