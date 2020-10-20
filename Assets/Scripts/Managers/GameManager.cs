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
    public static Action onLevelLoaded_01;  // For registering the player base
    public static Action onLevelLoaded_02;  // for registering enemies

    [SerializeField] private LevelMap levelMap;
    [SerializeField] private DNABase playerBase;
    [SerializeField] private List<BasicEnemy> enemyList = new List<BasicEnemy>();
    private GameObject enemyContainer;      

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

        enemyList.Add(enemy);
        enemy.SetMap(levelMap.map);
        enemy.agent.SetDestination(playerBase.transform.position);
        enemy.transform.parent = enemyContainer.transform;
    }

    public void UnregisterEnemy(BasicEnemy enemy)
    {
        enemy.StopMoving();
        enemyList.Remove(enemy);
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
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
