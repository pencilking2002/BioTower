using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public enum GameState
{
    START_MENU,
    GAME,
    GAME_OVER_WIN,
    GAME_OVER_LOSE,
    LOADING,
    PAUSE,
    UPGRADE_MENU
}
public class GameStates : MonoBehaviour
{
    public GameState gameState;

    public void SetStartMenuState() { gameState = GameState.START_MENU; }
    public void SetGameState() { gameState = GameState.GAME; }
    public void SetWinState() { gameState = GameState.GAME_OVER_WIN; }
    public void SetLoseState() { gameState = GameState.GAME_OVER_LOSE; }
    public void SetLoadingState() { gameState = GameState.LOADING; }
    public void SetPauseState() { gameState = GameState.PAUSE; }
    public void SetUpgradeMenuState() { gameState = GameState.UPGRADE_MENU; }

    public bool IsStartMenuState() { return gameState == GameState.START_MENU; }
    public bool IsGameState() { return gameState == GameState.GAME; }
    public bool IsWinState() { return gameState == GameState.GAME_OVER_WIN; }
    public bool IsLoseState() { return gameState == GameState.GAME_OVER_LOSE; }
    public bool IsLoadingState() { return gameState == GameState.LOADING; }
    public bool IsPauseState() { return gameState == GameState.PAUSE; }
    public bool IsUpgradeMenuState() { return gameState == GameState.UPGRADE_MENU; }
}
}
