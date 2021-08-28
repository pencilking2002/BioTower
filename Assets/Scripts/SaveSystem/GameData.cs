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
    //public List<TowerData> towerDataList;
    // public DNATowerData dnaTowerData;
    // public AbaTowerData abaTowerData;
    // public PPC2TowerData ppc2TowerData;
    //public ChloroplastTowerData chloroplastTowerData;
    public int currLevel;
    public bool enabledTowerHealthDecline;
    public int abaUnitSpawnLimit;
    public int startingEnergy;
    public int energy;

    public GameData()
    {
        abaUnitSpawnLimit = 3;
        enabledTowerHealthDecline = false;
        currLevel = 0;
        startingEnergy = 80;
        energy = 0;
    }
}

// public class DNATowerData
// {
//     public int maxHealth;
// }

// public class AbaTowerData
// {
//     public int maxHealth;
//     public int decayRate;
//     public int maxUnitHealth;
// }

// public class PPC2TowerData
// {
//     public int maxHealth;
//     public float decayRate;
//     public float shootInterval;
// }

// public class ChloroplastTowerData
// {
//     public int maxHealth;
//     public int decayRate;
//     public int upgradeLevel;
// }
}
