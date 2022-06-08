using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BioTower.Structures;
using BioTower.Units;
using BioTower.SaveData;
using UnityEngine.UI;
using BioTower.Level;

namespace BioTower
{
    public class EventManager : MonoBehaviour
    {
        public class Game
        {
            public static Action<bool, float> onGameOver;       // bool -> win/lose, float -> delay 
            public static Action onLevelLoaded_01;  // For registering the player base
            public static Action onLevelLoaded_02;  // for registering enemies

            public static Action<int, int> onSpendCurrency; // num spent, curr total
            public static Action<int, int> onGainCurrency;  // num gained, curr total
                                                            //public static Action onTogglePaths; 
            public static Action<GameState> onGameStateInit;
            public static Action onLightFragmentTapped;
            public static Action onCrystalTapped;
            public static Action<EnemyCrystal> onCrystalCreated;
            public static Action<EnemyCrystal> onCrystalDestroyed;
            public static Action<Snrk2Unit> onSnrk2UnitReachedBase;

            public static Action<LevelType> onLevelAwake;
            public static Action<LevelType> onLevelStart;
        }

        public class Wave
        {
            public static Action<WaveMode> onWaveStateInit;
            public static Action<int> onWaveCountdownTick;
            public static Action<Waypoint> onDisplayWaveWarning;
            public static Action<Waypoint> onStopWaveWarning;
            public static Action onWavesCompleted;
        }

        public class Structures
        {
            public static Action onBaseTakeDamage;
            public static Action onBaseDestroyed;
            public static Action<StructureType> onStartPlacementState;
            public static Action onSetNonePlacementState;
            public static Action<Structure, bool> onStructureCreated;
            public static Action<Structure> onStructureActivated;       // Used for activating structures that exist on the map at the beginning
            public static Action<Structure> onStructureDestroyed;
            public static Action<StructureType, float> onStructureCooldownStarted;
            public static Action<Structure> onStructureSelected;
            public static Action<Structure> onStructureGainHealth;
            public static Action<Structure> onStructureLoseHealth;
            public static Action onLightDropped;
            public static Action onLightPickedUp;
            public static Action<EnemyUnit, Structure> onEnemyEnterTowerInfluence;
            public static Action<EnemyUnit, Structure> onEnemyExitTowerInfluence;
            public static Action<StructureSocket> onSocketStart;
            public static Action<StructureSocket> onSocketPop;
            public static Action<StructureSocket> onTapFreeStructureSocket;
            public static Action onPlaceBomb;

        }

        public class Units
        {
            public static Action<Unit, Unit> onStartCombat;     // player unit, enemy unit
            public static Action<EnemyUnit> onEnemyBaseReached;
            public static Action<EnemyUnit> onEnemyReachedDestination;
            public static Action<Unit> onUnitSpawned;
            public static Action<Unit> onUnitDestroyed;
            public static Action<Snrk2Unit> onCrystalPickedUp;
            public static Action onEnemyPickedUpCrystal;
            public static Action<UnitType> onUnitTakeDamage;
            public static Action<int> onEnemyBarrierCollision;
        }

        public class UI
        {
            public static Action<StructureType> onPressTowerButton;
            public static Action onPressUpgradeButton;
            public static Action onTapLevelSelectButton;
            //public static Action<UnitType> onTapSpawnUnitButton;
            public static Action<bool> onTapButton;     // Is button press valid
            public static Action<MitoTower> onTapLightDropButton;
            public static Action<Structure> onSpawnLightDropCooldownComplete;
            public static Action onPressLevelSelectButton;
            public static Action onLetterReveal;
            public static Action<int> onTitleAnimCompleted;
            public static Action<Structure> onPressTowerDestroyedBtn;

        }

        public class Input
        {
            public static Action<Vector3> onTouchBegan;     // screen position of the touch
            public static Action onTapStartMenu;
            public static Action onTap;
            public static Action onTapNothing;
        }

        public class Tutorials
        {
            public static Action<TutState> onTutStateInit;
            public static Action<TutorialData> onTutorialStart;
            public static Action<TutorialData> onTutorialEnd;
            public static Action onTutTextPopUp;
            public static Action onTutChatStart;
            public static Action<HighlightedItem> onHighlightItem;
            public static Action<string, Vector2, float> onAnimateText;
            public static Action onSkipTutorials;
        }
    }
}