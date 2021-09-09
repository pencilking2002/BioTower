using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using System;

namespace BioTower.SaveData
{
[Serializable]
public class GameData
{
    public int currLevel;
    public bool enabledTowerHealthDecline;
    public int abaUnitSpawnLimit;
    public int startingEnergy;
    public int energy;

    // ABA Influence
    public AbaTowerSettings abaTowerSettings;
   

    // Set Default settings in the constructor
    public GameData()
    {
        abaUnitSpawnLimit = 3;
        enabledTowerHealthDecline = false;
        currLevel = 0;
        startingEnergy = 80;
        energy = 80;

        abaTowerSettings = new AbaTowerSettings();
    
        abaTowerSettings.SetAbaTowerInfluence(
            Util.gameSettings.abaMaxInfluenceRadius, 
            Util.gameSettings.abaMapScale, 
            Util.gameSettings.abaInfluenceShapeRadius
        );
        abaTowerSettings.SetAbaUnitCost(Util.gameSettings.abaUnitCost);
        abaTowerSettings.SetAbaUnitSpawnLimit(Util.gameSettings.abaUnitSpawnLimit);
    }
}
}
