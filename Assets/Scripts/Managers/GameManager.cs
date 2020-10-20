using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using BioTower.Units;

namespace BioTower
{
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action onLevelLoaded;
    [SerializeField] private List<BasicEnemy> enemyList = new List<BasicEnemy>();

    [SerializeField] private int numLoads;

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
    }

    private void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        numLoads++;
        Debug.Log($"Scene: {scene}. Mode: {mode}. Num Loads: {numLoads}");
        enemyList = new List<BasicEnemy>();
        onLevelLoaded?.Invoke();
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
