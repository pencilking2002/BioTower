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
    // ABA Influence
    public Params settings;
    

    // Keeps track of what upgrades the player has
    public List<ChosenUpgrade> chosenUpgrades;


    // Set Default settings in the constructor
    public GameData()
    {
        chosenUpgrades = new List<ChosenUpgrade>();
    }

    public void SetDefaultSettings(Params settings)
    {
        this.settings = settings;
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
