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
    public int energy;

    // ABA Influence
    public AbaTowerSettings abaTowerSettings;
    

    // Keeps track of what upgrades the player has
    public List<ChosenUpgrade> chosenUpgrades;


    // Set Default settings in the constructor
    public GameData()
    {
        var settings = Util.gameSettings;

        enabledTowerHealthDecline = settings.enableTowerHealthDecline;
        currLevel = settings.currLevel;
        energy = settings.startingEnergy;

        // ABA defaults
        abaTowerSettings = new AbaTowerSettings();
    
        abaTowerSettings.SetAbaTowerInfluence(
            Util.gameSettings.abaMaxInfluenceRadius, 
            Util.gameSettings.abaMapScale, 
            Util.gameSettings.abaInfluenceShapeRadius
        );
        abaTowerSettings.SetAbaUnitCost(Util.gameSettings.abaUnitCost);
        abaTowerSettings.SetAbaUnitSpawnLimit(Util.gameSettings.abaUnitSpawnLimit);
        abaTowerSettings.SetAbaUnitHealth(Util.gameSettings.abaUnitMaxHealth);

        chosenUpgrades = new List<ChosenUpgrade>();
    }
}

 [Serializable]
    public class ChosenUpgrade
    {
        public int level;
        public int varIndex;

        public ChosenUpgrade(int level, int varIndex)
        {
            this.level = level;
            this.varIndex = varIndex;
        }
    }
}
