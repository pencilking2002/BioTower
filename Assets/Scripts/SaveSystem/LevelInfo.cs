using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BioTower.SaveData
{
public enum LevelType
{
    LEVEL_00, LEVEL_01, LEVEL_02, LEVEL_03, LEVEL_04, LEVEL_05,
    LEVEL_06, LEVEL_07, LEVEL_08, LEVEL_09, LEVEL_10, LEVEL_11,
    LEVEL_12, LEVEL_13, LEVEL_14, LEVEL_15
}   

public class LevelInfo : MonoBehaviour
{    
    public static LevelInfo current => GameObject.FindGameObjectWithTag(Constants.levelInfo).GetComponent<LevelInfo>();
    public LevelSettings waveSettings;
    //public int levelIndex;
    public LevelType levelType;
    public WinCondition winCondition;
    public int numEnemiesToDestroy;
    public int numEnemiesDestroyed;
    public LoseCondition loseCondition;

    private void Awake()
    {
        EventManager.Game.onLevelAwake?.Invoke(levelType);
    }

    private void Start()
    {
        var saveData = GameManager.Instance.saveManager.Load();
        if (levelType == LevelType.LEVEL_00)
        {
            saveData.enabledTowerHealthDecline = false;
            GameManager.Instance.gameSettings.enableTowerHealthDecline = saveData.enabledTowerHealthDecline;
            GameManager.Instance.gameSettings.energy = saveData.startingEnergy;
        }
        else
        {
            saveData.enabledTowerHealthDecline = true;
            GameManager.Instance.gameSettings.enableTowerHealthDecline = saveData.enabledTowerHealthDecline;
            GameManager.Instance.gameSettings.energy = saveData.energy;
        }

        GameManager.Instance.gameSettings.abaUnitSpawnLimit = saveData.abaUnitSpawnLimit;
        GameManager.Instance.saveManager.Save(saveData);
        GameManager.Instance.econManager.Init(levelType);
    }
}
}