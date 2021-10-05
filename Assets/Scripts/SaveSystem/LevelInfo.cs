using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BioTower.SaveData
{
public enum LevelType
{
    NONE, LEVEL_01, 
    LEVEL_02, LEVEL_03, 
    LEVEL_04, LEVEL_05, 
    LEVEL_06, LEVEL_07, 
    LEVEL_08, LEVEL_09, 
    LEVEL_10, LEVEL_11,
    LEVEL_12, LEVEL_13, 
    LEVEL_14, LEVEL_15
}   

public class LevelInfo : MonoBehaviour
{    
    public static LevelInfo current => GameObject.FindGameObjectWithTag(Constants.levelInfo).GetComponent<LevelInfo>();
    public LevelSettings waveSettings;    
    public WinCondition winCondition;
    public LoseCondition loseCondition;
    public LevelType levelType;
    public int numEnemiesToDestroy;
    public int numEnemiesDestroyed;

    private void Awake()
    {
        EventManager.Game.onLevelAwake?.Invoke(levelType);
    }

    private void Start()
    {
        var saveData = GameManager.Instance.saveManager.Load();
        if (levelType == LevelType.LEVEL_01)
        {
            InitializeFirstLevel(ref saveData);
        }
        else
        {
           InitializeLevel(ref saveData);
        }

        GameManager.Instance.saveManager.Save(saveData);
        EventManager.Game.onLevelStart?.Invoke(levelType);
    }

    /// <summary>
    /// Established defaults for tower health decline and starting energy
    /// </summary>
    /// <param name="saveData"></param>
    /// <returns></returns>
    private void InitializeFirstLevel(ref GameData saveData)
    {
        //saveData = new GameData();
        var defaultSettings = Util.gameSettings.defaultSettings;
        var upgradeSettings = Util.gameSettings.upgradeSettings;
        Util.gameSettings.SetUpgradeSettingsToDefault();
        saveData.SetDefaultSettings(defaultSettings);
        Debug.Log("Init first lvl. energy: " + upgradeSettings.energy);
        GameManager.Instance.econManager.Init(Util.gameSettings.startingEnergy);
    }

    /// <summary>
    /// Loads data for any level that's not the intro level.
    /// Enables tower health decline, user's energy, aba unit spawn limit 
    /// </summary>
    /// <param name="saveData"></param>
    /// <returns></returns>
    private void InitializeLevel(ref GameData saveData)
    {
        saveData.settings.enableTowerHealthDecline = true;
        Util.gameSettings.SetUpgradeSettingsBasedOnGameData(saveData);
        GameManager.Instance.econManager.Init(Util.gameSettings.upgradeSettings.energy);

        //var settings = GameManager.Instance.gameSettings;
        //settings.enableTowerHealthDecline = true;
        //settings.energy = saveData.energy;
        //settings.abaUnitSpawnLimit = saveData.abaUnitSpawnLimit;
        //settings.abaUnitMaxHealth = saveData.abaTowerSettings.abaUnitMaxHealth;
    }
}
}