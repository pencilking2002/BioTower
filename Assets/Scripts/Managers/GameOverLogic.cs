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
            EventManager.Game.onGameOver?.Invoke(false);
        }
    }

    private void OnUnitDestroyed(Unit unit)
    {
        if (unit.unitType != UnitType.BASIC_ENEMY)
            return;
            
        var levelInfo = LevelInfo.current;
        if (levelInfo.winCondition == WinCondition.KILL_ENEMIES)
        {
            levelInfo.numEnemiesDestroyed++;   
            if (levelInfo.numEnemiesDestroyed >= levelInfo.numEnemiesToDestroy)
            {
                EventManager.Game.onGameOver(true);
            }
        }
    }

    private void OnWavesCompleted()
    {
        if (LevelInfo.current.winCondition == WinCondition.SURVIVE_WAVES)
        {
            EventManager.Game.onGameOver?.Invoke(true);
        }
    }

    private void OnSpendCurrency(int numSpent, int currPlayerCurrency)
    {
        if (currPlayerCurrency == 0)
        {
            EventManager.Game.onGameOver?.Invoke(false);
        }
    }

    private void OnEnable()
    {
        EventManager.Structures.onBaseDestroyed += OnBaseDestroyed;
        EventManager.Game.onWavesCompleted += OnWavesCompleted;
        EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
        EventManager.Game.onSpendCurrency += OnSpendCurrency;
    }

    private void OnDisable()
    {
        EventManager.Structures.onBaseDestroyed -= OnBaseDestroyed;
        EventManager.Game.onWavesCompleted -= OnWavesCompleted;
        EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
        EventManager.Game.onSpendCurrency -= OnSpendCurrency;
    }
    
}
}