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
    public DNATowerData dnaTowerData;
    public AbaTowerData abaTowerData;
    public PPC2TowerData ppc2TowerData;
    public ChloroplastTowerData chloroplastTowerData;
}

public class DNATowerData
{
    public int maxHealth;
}

public class AbaTowerData
{
    public int maxHealth;
    public int decayRate;
    public int maxUnitHealth;
}

public class PPC2TowerData
{
    public int maxHealth;
    public float decayRate;
    public float shootInterval;
}

public class ChloroplastTowerData
{
    public int maxHealth;
    public int decayRate;
    public int upgradeLevel;
}
}
