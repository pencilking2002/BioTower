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

    public static Action<bool> onGameOver;
    public static Action onLevelLoaded_01;  // For registering the player base
    public static Action onLevelLoaded_02;  // for registering enemies

    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private LevelMap levelMap;
    public DNABase playerBase;
    [SerializeField] private List<BasicEnemy> enemyList = new List<BasicEnemy>();
    private GameObject enemyContainer;  

    private WaveManager _waveMananger;
    public WaveManager waveManager
    {
        get
        {
            if (!_waveMananger)
                _waveMananger = GameObject.FindGameObjectWithTag(Constants.waveManager).GetComponent<WaveManager>();
            
            return _waveMananger;
        }
    }    

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
        enemy.agent.SetDestination(playerBase.transform.position);
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
       
        onLevelLoaded_01?.Invoke();
        onLevelLoaded_02?.Invoke();
    }

    private void OnBaseDestroyed()
    {
        onGameOver?.Invoke(false);
    }

    private void OnWavesCompleted()
    {
        onGameOver?.Invoke(true);
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DNABase.onBaseDestroyed += OnBaseDestroyed;
        WaveState.onWavesCompleted += OnWavesCompleted;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DNABase.onBaseDestroyed -= OnBaseDestroyed;
        WaveState.onWavesCompleted -= OnWavesCompleted;
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
