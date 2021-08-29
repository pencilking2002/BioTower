using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.SaveData
{
public class LevelInfo : MonoBehaviour
{    
    public static LevelInfo current => GameObject.FindGameObjectWithTag(Constants.levelInfo).GetComponent<LevelInfo>();
    public LevelSettings waveSettings;
    public int levelIndex;
    public WinCondition winCondition;
    public LoseCondition loseCondition;

    private void Awake()
    {
        EventManager.Game.onLevelAwake?.Invoke(levelIndex);
    }

    private void Start()
    {
        var saveData = GameManager.Instance.saveManager.Load();
        if (levelIndex == 0)
        {
            saveData.enabledTowerHealthDecline = false;
            GameManager.Instance.gameSettings.startingEnergy = saveData.startingEnergy;
        }
        else
            saveData.enabledTowerHealthDecline = true;
        
        GameManager.Instance.saveManager.Save(saveData);
        GameManager.Instance.gameSettings.enableTowerHealthDecline = saveData.enabledTowerHealthDecline;
        GameManager.Instance.gameSettings.abaUnitSpawnLimit = saveData.abaUnitSpawnLimit;

        GameManager.Instance.econManager.Init(levelIndex);
    }
}
}