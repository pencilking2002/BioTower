using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using BioTower.UI;

namespace BioTower
{
public class  BootController : MonoBehaviour
{
    public static int levelToLoadInstantly = -1;

    [Header("Game State")]
    [ShowInInspector] public static bool isBootLoaded;
    public GameState gameState;
    [SerializeField] private int sceneToLoad;
    

    [Header("References")]
    public WavePanel wavePanel;
    public GameCanvas gameCanvas;
    public StartMenuCanvas startMenuCanvas;
    public GameplayUI gameplayUI;
    public UpgradePanel upgradePanel;
    public TowerMenu towerMenu;
    public LevelSelectMenu levelSelectMenu;
    public bool isBootOrMenuScene => SceneManager.GetActiveScene().name == Constants.boot;
    private Dictionary<GameState, BootStateBase> charStates = new Dictionary<GameState, BootStateBase>();
    [ReadOnly] public bool isLoading;

    private void Awake()
    {
        CacheStates();
        if (levelToLoadInstantly != -1)
        {
            levelToLoadInstantly = -1;
            gameState = GameState.LEVEL_SELECT;
        }
    }

    // public void LoadFirstScene()
    // {
    //     BootController.isBootLoaded = true;
    //     SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    // }

    private void Start()
    {
        
    }

    private void Update()
    {
        gameState = charStates[gameState].OnUpdate(gameState);
        GameManager.Instance.gameStates.gameState = gameState;
    }

    private void CacheStates()
    {
        var states = GetComponents<BootStateBase>();
        foreach(BootStateBase state in states)
        {
            charStates.Add(state.gameState, state);
        }
    }

    public void SetGameState()
    {
        gameState = GameState.GAME;
        GameManager.Instance.gameStates.SetGameState();
    } 
    
}
}