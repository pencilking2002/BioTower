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
public class BootController : MonoBehaviour
{
        
    [Header("Game State")]
    [ShowInInspector] public static bool isBootLoaded;
    public GameState gameState;
    [SerializeField] private int sceneToLoad;
    
    [Header("References")]
    public GameCanvas gameCanvas;
    public GameplayUI gameplayUI;
    public UpgradePanel upgradePanel;
    public bool isBootOrMenuScene => SceneManager.GetActiveScene().name == Constants.boot;
    private Dictionary<GameState, BootStateBase> charStates = new Dictionary<GameState, BootStateBase>();
    [ReadOnly] public bool isLoading;

    private void Awake()
    {
        CacheStates();

        // if (!isBootLoaded)
        // {
        //     isBootLoaded = true;
        //     SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        // }
    }

    public void LoadFirstScene()
    {
        BootController.isBootLoaded = true;
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

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
    
}
}