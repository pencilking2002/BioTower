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
        GameManager.onLevelLoaded_01 += LevelLoaded;
    }

    private void OnDisable()
    {
        GameManager.onLevelLoaded_01 -= LevelLoaded;
    }
}
}
