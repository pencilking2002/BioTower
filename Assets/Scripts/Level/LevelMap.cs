using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

namespace BioTower.Level
{
public class LevelMap : MonoBehaviour
{
    public PolyNav2D map;

    private void LevelLoaded()
    {
        GameManager.Instance.RegisterMap(this);
    }   

    private void OnEnable()
    {
        EventManager.Game.onLevelLoaded_01 += LevelLoaded;
    }

    private void OnDisable()
    {
        EventManager.Game.onLevelLoaded_01 -= LevelLoaded;
    }
}
}
