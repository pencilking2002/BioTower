using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.SaveData
{
public class LevelInfo : MonoBehaviour
{
    public int levelIndex;
    public static LevelInfo current => GameObject.FindGameObjectWithTag(Constants.levelInfo).GetComponent<LevelInfo>();

    private void Awake()
    {
        EventManager.Game.onLevelAwake?.Invoke(levelIndex);
    }
}
}