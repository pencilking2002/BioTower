using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    // [SerializeField] private int numEnemiesKilled;
    // [SerializeField] private int numWavesSurvived;
    // [SerializeField] private bool baseWasDestroyed;

    // private Dictionary<WinCondition, Func<int, bool>> winConditionMap = new Dictionary<WinCondition, Func<int, bool>>();
    // private Dictionary<LoseCondition, Func<int, bool>> loseConditionMap = new Dictionary<LoseCondition, Func<int, bool>>();


    private void Awake()
    {
        // winConditionMap.Add(WinCondition.SURVIVE_WAVES, DidSurviveWaves);
        // winConditionMap.Add(WinCondition.KILL_ENEMIES, DidKillEnemies);

        // loseConditionMap.Add(LoseCondition.NONE, NoneLoseCondition);
        // loseConditionMap.Add(LoseCondition.BASE_DESTROYED, DidBaseGetDestroyed);
    }

    // public bool CheckWinCondition(WinCondition winCondition, int inputNum)
    // {
    //     return winConditionMap[winCondition](inputNum);
    // }

    // private bool DidSurviveWaves(int numWavesSurvived)
    // {
    //     return this.numWavesSurvived == numWavesSurvived;
    // }

    // private bool DidKillEnemies(int numEnemiesKilled)
    // {
    //     return (this.numEnemiesKilled == numEnemiesKilled);
    // }

    // public bool CheckLoseCondition(LoseCondition loseCondition)
    // {
    //     return loseConditionMap[loseCondition](0);
    // }

    // private bool NoneLoseCondition(int num)
    // {
    //     return false;
    // }

    // private bool DidBaseGetDestroyed(int num)
    // {
    //     return baseWasDestroyed;
    // }

    private void OnBaseDestroyed()
    {
        if (LevelInfo.current.loseCondition == LoseCondition.BASE_DESTROYED)
        {
            EventManager.Game.onGameOver?.Invoke(false);
        }
    }

    private void OnWavesCompleted()
    {
        if (LevelInfo.current.winCondition == WinCondition.SURVIVE_WAVES)
        {
            EventManager.Game.onGameOver?.Invoke(true);
        }
    }

    private void OnEnable()
    {
        EventManager.Structures.onBaseDestroyed += OnBaseDestroyed;
        EventManager.Game.onWavesCompleted += OnWavesCompleted;
    }

    private void OnDisable()
    {
        EventManager.Structures.onBaseDestroyed -= OnBaseDestroyed;
        EventManager.Game.onWavesCompleted -= OnWavesCompleted;
    }
    
}
}