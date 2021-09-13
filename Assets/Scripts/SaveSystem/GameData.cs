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
    //public int abaUnitSpawnLimit;
    public int energy;

    // ABA Influence
    public Params settings;
    

    // Keeps track of what upgrades the player has
    public List<ChosenUpgrade> chosenUpgrades;


    // Set Default settings in the constructor
    public GameData()
    {
        currLevel = Util.gameSettings.defaultSettings.startingLevel;
        enabledTowerHealthDecline = Util.gameSettings.defaultSettings.enableTowerHealthDecline;
        energy = Util.gameSettings.defaultSettings.startingEnergy;

        // ABA defaults
        // abaTowerSettings = new AbaTowerSettings();
    
        // abaTowerSettings.SetAbaTowerInfluence(
        //     Util.gameSettings.defaultSettings.abaMaxInfluenceRadius, 
        //     Util.gameSettings.defaultSettings.abaMapScale, 
        //     Util.gameSettings.defaultSettings.abaInfluenceShapeRadius
        // );
        // abaTowerSettings.SetAbaUnitCost(Util.gameSettings.defaultSettings.abaUnitCost);
        // abaTowerSettings.SetAbaUnitSpawnLimit(Util.gameSettings.defaultSettings.abaUnitSpawnLimit);
        // abaTowerSettings.SetAbaUnitHealth(Util.gameSettings.defaultSettings.abaUnitMaxHealth);
        settings = Util.gameSettings.defaultSettings;
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
