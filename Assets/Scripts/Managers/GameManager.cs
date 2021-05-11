using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using BioTower.Units;
using BioTower.Structures;
using PolyNav;
using BioTower.Level;

namespace BioTower
{
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameSettings gameSettings;


    [Header("Scene References")]
    public LevelMap levelMap;
    public DNABase playerBase;
    [SerializeField] private List<BasicEnemy> enemyList = new List<BasicEnemy>();
    private GameObject enemyContainer;  


    [Header("References")]
    public WaveManager waveManager;
    public PlacementManager placementManager;
    public EconomyManager econManager;

    private WaypointManager waypointManager;

    private void Awake()
    {
        InitInstance();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadLevel(0);
        }  

        Time.timeScale = gameSettings.timeScale;  
    }

    public WaypointManager GetWaypointManager()
    {
        if (!waypointManager)
            waypointManager = GameObject.FindGameObjectWithTag(Constants.waypointManager).GetComponent<WaypointManager>();
        
        return waypointManager;
    }

    public void RegisterMap(LevelMap levelMap)
    {
        this.levelMap = levelMap;
    }

    /// <summary>
    /// Registers enemy in list and 
    /// sets player base as the target
    /// </summary>
    /// <param name="enemy"></param>
    public void RegisterEnemy(BasicEnemy enemy)
    {
        if (enemyList.Contains(enemy) || playerBase == null)
            return;

        enemy.transform.parent = enemyContainer.transform;
        enemyList.Add(enemy);
        enemy.agent.map = levelMap.map;
        Debug.Log("Register enemy agent: " + enemy.gameObject.name + ". map: " + levelMap.map.gameObject.name + ". destination: " + playerBase.gameObject.name);
    }

    public void UnregisterEnemy(BasicEnemy enemy)
    {
        enemy.StopMoving();
        enemyList.Remove(enemy);
        Debug.Log("Unregister enemy");
    }

    public void RegisterPlayerBase(DNABase playerbase)
    {
        playerBase = playerbase;
    }

    public void LoadLevel(int sceneIndex)
    {
        for (int i=0; i<enemyList.Count; i++)
            UnregisterEnemy(enemyList[i]);
        
        enemyContainer = null;
        playerBase = null;
        SceneManager.LoadScene(sceneIndex);
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log($"Scene: {scene}. Mode: {mode}");
        enemyContainer = GameObject.FindGameObjectWithTag(Constants.enemyContainer);
        enemyList = new List<BasicEnemy>();
       
        EventManager.Game.onLevelLoaded_01?.Invoke();
        EventManager.Game.onLevelLoaded_02?.Invoke();
    }

    private void OnBaseDestroyed()
    {
        EventManager.Game.onGameOver?.Invoke(false);
    }

    private void OnWavesCompleted()
    {
         EventManager.Game.onGameOver?.Invoke(true);
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager.Structures.onBaseDestroyed += OnBaseDestroyed;
        EventManager.Game.onWavesCompleted += OnWavesCompleted;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventManager.Structures.onBaseDestroyed -= OnBaseDestroyed;
        EventManager.Game.onWavesCompleted -= OnWavesCompleted;
    }

    private void InitInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            
            if (transform.parent != null)
                DontDestroyOnLoad(gameObject.transform.parent.gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
}
