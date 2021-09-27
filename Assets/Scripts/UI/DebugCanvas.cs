using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyNav;
using BioTower.Structures;
using System;

namespace BioTower.UI
{
public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private GameObject enemyPrefab;

    // [SerializeField] private PolyNav2D map;
    // [SerializeField] private Transform endPoint;


    [Header("Text")]
    [SerializeField] private Text currWaveText;
    [SerializeField] private Text placementStateText;
    //[SerializeField] private GameObject testMapPrefab;
  

    private void Awake()
    {
        placementStateText.text = "Placement state: NONE";
    }
    
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     SpawnEnemy();
        // }
        if (Input.GetKeyDown(KeyCode.D))
        {
            panel.gameObject.SetActive(!panel.gameObject.activeInHierarchy);
        }

        currWaveText.text = "Curr Wave: " + GameManager.Instance.waveManager.currWave;
    }

    // public void SpawnTestMap()
    // {
    //     Instantiate(testMapPrefab);
    // }

    public void SpawnEnemy()
    {
        var enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.position = GameObject.FindGameObjectWithTag(Constants.enemyTestSpawnSpot).transform.position;
    }

    public void ReloadLevel()
    {
        GameManager.Instance.LoadLevel(0);
    }

    private void OnStartPlacementState(StructureType structureType)
    {
        placementStateText.text = "Placement state: PLACING";
    }

    private void OnSetNonePlacementState()
    {
        placementStateText.text = "Placement state: NONE";
    }

    public void UnlockAllTowers()
    {
        var gameData = Util.saveManager.Load();
        gameData.settings.mitoTowerUnlocked = true;
        gameData.settings.ppc2TowerUnlocked = true;
        gameData.settings.chloroTowerUnlocked = true;
        Util.saveManager.Save(gameData);
        GameManager.Instance.upgradeSettings = gameData.settings;
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.PPC2_TOWER].gameObject.SetActive(true);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.CHLOROPLAST].gameObject.SetActive(true);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.MITOCHONDRIA].gameObject.SetActive(true);
    }

    public void ResetTowers()
    {
        var gameData = Util.saveManager.Load();
        gameData.settings.mitoTowerUnlocked = false;
        gameData.settings.ppc2TowerUnlocked = false;
        gameData.settings.chloroTowerUnlocked = false;
        Util.saveManager.Save(gameData);
        GameManager.Instance.upgradeSettings = gameData.settings;
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.PPC2_TOWER].gameObject.SetActive(false);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.CHLOROPLAST].gameObject.SetActive(false);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.MITOCHONDRIA].gameObject.SetActive(false);
    }

    public void UpgradeAll()
    {
        foreach(KeyValuePair<UpgradeType, Action> item in Util.gameSettings.upgradeLogicMap)
            item.Value();
        
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.PPC2_TOWER].gameObject.SetActive(true);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.CHLOROPLAST].gameObject.SetActive(true);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.MITOCHONDRIA].gameObject.SetActive(true);
        Debug.Log("Unlock all");
    }

    public void UnlockAllLevels()
    {
        var buttons = Util.bootController.levelSelectMenu.GetButtons();
        Util.upgradeSettings.currLevel = buttons.Length;
        foreach(var btn in buttons)
        {
            btn.Unlock();
        }
    }

    public void Gain100Energy()
    {
        Util.econManager.GainCurrency(100);
    }

    public void Gain1000Energy()
    {
        Util.econManager.GainCurrency(1000);
    }

    public void SetEnergyToZero()
    {
        Util.econManager.SpendCurrency(100000000);
    }

    private void OnEnable()
    {
        EventManager.Structures.onSetNonePlacementState += OnSetNonePlacementState;
        EventManager.Structures.onStartPlacementState += OnStartPlacementState;
    }

    private void OnDisable()
    {
        EventManager.Structures.onSetNonePlacementState -= OnSetNonePlacementState;
        EventManager.Structures.onStartPlacementState -= OnStartPlacementState;
    }
}
}
