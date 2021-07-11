using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BioTower.Structures;
using BioTower.Units;

namespace BioTower
{
public class EventManager : MonoBehaviour
{
    public class Game
    {
        public static Action<bool> onGameOver;
        public static Action onLevelLoaded_01;  // For registering the player base
        public static Action onLevelLoaded_02;  // for registering enemies
        public static Action onWavesCompleted;

        public static Action<int, int> onSpendCurrency; // num spent, curr total
        public static Action<int, int> onGainCurrency;  // num gained, curr total
        public static Action onTogglePaths; 
        public static Action<GameState> onGameStateInit;
    }

    public class Structures
    {
        public static Action onBaseDestroyed;
        public static Action<StructureType> onStartPlacementState;
        public static Action onSetNonePlacementState;
        public static Action<Structure> onStructureCreated;
        public static Action<Structure> onStructureDestroyed;
        public static Action<StructureType, float> onStructureCooldownStarted;
        public static Action<Structure> onStructureSelected;
    }

    public class Units
    {
        public static Action onEnemyBaseReached;
        public static Action<BasicEnemy> onEnemyReachedDestination;
    }

    public class UI
    {
        public static Action<StructureType> onPressTowerButton;
        public static Action onPressUpgradeButton;
    }

    public class Input
    {
        public static Action<Vector3> onTouchBegan;     // screen position of the touch
        public static Action onTapStartMenu;
    }
}
}