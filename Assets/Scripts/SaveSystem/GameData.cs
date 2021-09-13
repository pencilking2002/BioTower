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
    public AbaTowerSettings abaTowerSettings;
    

    // Keeps track of what upgrades the player has
    public List<ChosenUpgrade> chosenUpgrades;


    // Set Default settings in the constructor
    public GameData()
    {
        currLevel = GameSettings.startingLevel;
        enabledTowerHealthDecline = GameSettings.enableTowerHealthDecline;
        energy = GameSettings.startingEnergy;

        // ABA defaults
        abaTowerSettings = new AbaTowerSettings();
    
        abaTowerSettings.SetAbaTowerInfluence(
            GameSettings.abaMaxInfluenceRadius, 
            GameSettings.abaMapScale, 
            GameSettings.abaInfluenceShapeRadius
        );
        abaTowerSettings.SetAbaUnitCost(GameSettings.abaUnitCost);
        abaTowerSettings.SetAbaUnitSpawnLimit(GameSettings.abaUnitSpawnLimit);
        abaTowerSettings.SetAbaUnitHealth(GameSettings.abaUnitMaxHealth);

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
