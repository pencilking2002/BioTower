using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace BioTower
{
public class BootController : MonoBehaviour
{
        
    [Header("Game State")]
    [SerializeField] private GameState gameState;
    [SerializeField] private int sceneToLoad;
    
    [ShowInInspector] public static bool isBootLoaded;
    // [ShowInInspector] public static bool isPlayerLoaded;
    // [ShowInInspector] public static bool isSceneReloaded;

    public bool isBootOrMenuScene => SceneManager.GetActiveScene().name == Constants.boot;
    //private Dictionary<GameState, BootStateBase> charStates = new Dictionary<GameState, BootStateBase>();
    //[SerializeField] private SceneReference bootScene;
    [ReadOnly] public bool isLoading;

    private void Awake()
    {
        if (!isBootLoaded)
        {
            isBootLoaded = true;
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        }
    }

    private void Start()
    {
        // if (isBootOrMenuScene)
        //     SetMainMenuState();
        // else
        //     SetLevelState();
        
        // DontDestroyOnLoad(gameObject);
    }

    // public void OnPressQuitButton()
    // {
    //     Application.Quit();
    // }
 
    
}
}