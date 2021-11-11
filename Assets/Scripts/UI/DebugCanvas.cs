using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyNav;
using BioTower.Structures;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


namespace BioTower.UI
{
public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private bool enableOnAwake;
    [SerializeField] private RectTransform panel;
    [SerializeField] private GameObject enemyPrefab;


    [Header("Text")]
    [SerializeField] private Text currWaveText;
    [SerializeField] private Text placementStateText;
    [SerializeField] private Text tutorialText;
    [SerializeField] private Text tutIndexText;
    [SerializeField] private Text tutInProgressText;

    
    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;
    public Slider sfxSlider;
    public Slider musicSlider;
       
    private void Awake()
    {
        panel.gameObject.SetActive(enableOnAwake);
        placementStateText.text = "Placement state: NONE";
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            panel.gameObject.SetActive(!panel.gameObject.activeInHierarchy);
        }

        if (GameManager.Instance != null && GameManager.Instance.gameStates.IsGameState() && Util.tutCanvas != null)
        {
            currWaveText.text = "Curr Wave: " + GameManager.Instance.waveManager.currWave;
            tutorialText.text = $"Tut State: {Util.tutCanvas.tutState}";
            tutIndexText.text = $"Tut Index: {Util.tutCanvas.currTutorialIndex}";
            tutInProgressText.text = $"Tut in progress: {TutorialCanvas.tutorialInProgress}";
        }
    }

    public void SetFSXVolume()
    {
        var val = 20.0f * Mathf.Log10(sfxSlider.value);
        mixer.SetFloat("SfxVolume", val);
    }
    
    public void SetMusicVolume()
    {
        var val = 20.0f * Mathf.Log10(musicSlider.value);
        mixer.SetFloat("MusicVolume", val);
    }

    public void SpawnEnemy()
    {
        var enemy = Util.waveManager.SpawnEnemy(new Vector2(0.4f, 0.7f));
        enemy.transform.position = GameObject.FindGameObjectWithTag(Constants.enemyTestSpawnSpot).transform.position;
    }

    public void ReloadLevel()
    {
       Util.ReloadLevel();
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
        Util.upgradeSettings.mitoTowerUnlocked = true;
        Util.upgradeSettings.ppc2TowerUnlocked = true;
        Util.upgradeSettings.chloroTowerUnlocked = true;
     
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.PPC2_TOWER].gameObject.SetActive(true);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.CHLOROPLAST].gameObject.SetActive(true);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.MITOCHONDRIA].gameObject.SetActive(true);
        
        Util.saveManager.LoadAndSave();

    }

    public void ResetTowers()
    {
        Util.upgradeSettings.mitoTowerUnlocked = false;
        Util.upgradeSettings.ppc2TowerUnlocked = false;
        Util.upgradeSettings.chloroTowerUnlocked = false;

        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.PPC2_TOWER].gameObject.SetActive(false);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.CHLOROPLAST].gameObject.SetActive(false);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.MITOCHONDRIA].gameObject.SetActive(false);
        
        Util.saveManager.LoadAndSave();
    }

    public void UpgradeAll()
    {
        foreach(KeyValuePair<UpgradeType, Action> item in Util.gameSettings.upgradeLogicMap)
            item.Value();
        
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.PPC2_TOWER].gameObject.SetActive(true);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.CHLOROPLAST].gameObject.SetActive(true);
        GameManager.Instance.bootController.gameplayUI.towerButtonMap[StructureType.MITOCHONDRIA].gameObject.SetActive(true);
        
        Util.saveManager.LoadAndSave();
    }

    public void UnlockAllLevels()
    {
        var buttons = Util.bootController.levelSelectMenu.GetButtons();
        //Util.upgradeSettings = Util.gameSettings;

        Util.upgradeSettings.currLevel = buttons.Length;
        Util.saveManager.LoadAndSave();

        foreach(var btn in buttons)
            btn.Unlock();
    }

    public void ResetAllLevels()
    {
        var buttons = Util.bootController.levelSelectMenu.GetButtons();
        Util.upgradeSettings.currLevel = 1;
        Util.saveManager.LoadAndSave();

        for (int i=0; i<buttons.Length; i++)
        {
            if (i == 0)
                buttons[i].Unlock();
            else
                buttons[i].Lock();
        }
    }

    public void Gain100Energy()
    {
        Util.econManager.GainCurrency(100);
        Util.saveManager.LoadAndSave();
    }

    public void Gain1000Energy()
    {
        Util.econManager.GainCurrency(1000);
        Util.saveManager.LoadAndSave();
    }

    public void SetEnergyToZero()
    {
        Util.econManager.SpendCurrency(100000000);
        Util.saveManager.LoadAndSave();
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
