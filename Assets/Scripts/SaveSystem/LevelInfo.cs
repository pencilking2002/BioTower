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

    private void Awake()
    {
        EventManager.Game.onLevelAwake?.Invoke(levelIndex);
    }

    private void Start()
    {
        var saveData = GameManager.Instance.saveManager.Load();
        if (levelIndex == 0)
            saveData.enabledTowerHealthDecline = false;
        else
            saveData.enabledTowerHealthDecline = true;
        
        GameManager.Instance.saveManager.Save(saveData);
    }
}
}