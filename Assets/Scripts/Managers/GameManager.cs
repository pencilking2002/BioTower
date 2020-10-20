using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using BioTower.Units;
using BioTower.Structures;

namespace BioTower
{
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action onLevelLoaded_01;  // For registering the player base
    public static Action onLevelLoaded_02;  // for registering enemies

    [SerializeField] private DNABase playerBase;
    [SerializeField] private List<BasicEnemy> enemyList = new List<BasicEnemy>();

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

    public void RegisterEnemy(BasicEnemy enemy)
    {
        enemyList.Add(enemy);
        SetTarget(enemy);
    }

    public void RegisterPlayerBase(DNABase playerbase)
    {
        playerBase = playerbase;
    }

    private void SetTarget(BasicEnemy enemy)
    {
        enemy.SetTarget(playerBase.transform);
    }

    private void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log($"Scene: {scene}. Mode: {mode}");
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
