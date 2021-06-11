using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BioTower.Structures;

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
    }

    public class Structures
    {
        public static Action onBaseDestroyed;
        public static Action<StructureType> onStartPlacementState;
        public static Action onSetNonePlacementState;
        public static Action<StructureType> onStructureCreated;
        public static Action<StructureType, float> onStructureCooldownStarted;
    }

    public class Units
    {
        public static Action onEnemyBaseReached;
    }

    public class UI
    {
        public static Action<StructureType> onPressTowerButton;
    }

    public class Input
    {
        public static Action<Vector3> onTouchBegan;     // screen position of the touch
    }
}
}