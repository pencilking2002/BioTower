using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BioTower.SaveData;
using BioTower.Structures;
using BioTower.Units;
using System;

namespace BioTower
{
[CreateAssetMenu(fileName="UpgradeSettings", menuName="UpgradeSettings")]
[Serializable]
public class Params
{

    [Header("Units")]
    [TabGroup("Units")] public int ppc2Damage = 5;
    [TabGroup("Units")] public int abaDamage = 5;
    [TabGroup("Units")] public int basicEnemyDamage = 5;
    [TabGroup("Units")] public int abaUnitMaxHealth = 10;
    [TabGroup("Units")] public int snrkUnitMaxHealth = 5;
    [TabGroup("Units")] public int enemyUnitMaxHealth = 10;
    [TabGroup("Units")] public int abaUnitCost = 5;
    [TabGroup("Units")] public int snark2UnitCost = 1;


    [Header("Towers")]
    [TabGroup("Towers")] public int ppc2TowerCost = 40;
    [TabGroup("Towers")] public int chloroplastTowerCost = 20;
    [TabGroup("Towers")] public int mitoTowerCost = 20;
    [TabGroup("Towers")] public int healTowerCost = 5;
    [TabGroup("Towers")] public int spawnLightDropCost = 1;
    [TabGroup("Towers")] public int healTowerAmount = 2;
    [TabGroup("Towers")] public bool enableTowerHealthDecline = false;


    [Header("ABA Tower")]
    [TabGroup("Towers")][Range(0,100)] public int abaTowerCost = 10;
    [TabGroup("Towers")] public int abaUnitSpawnLimit = 3;
    [TabGroup("Towers")] public int abaMaxInfluenceRadius = 2310;
    [TabGroup("Towers")] public int abaMapScale = 2310;
    [TabGroup("Towers")] public int abaInfluenceShapeRadius = 2300;
    [TabGroup("Towers")] public bool enableAbaTowerRandomHeal = false;

    
    [TabGroup("Misc")] public int startingLevel = 1;
    [TabGroup("Misc")] public int currLevel = 1;
    [TabGroup("Misc")] public int lightFragmentValue = 5;
    [TabGroup("Misc")] public int crystalSnrk2Value = 20;
    [TabGroup("Misc")] public int startingEnergy = 80;
    [TabGroup("Misc")] public int energy = 80;


    public int GetUnitCost(UnitType unitType)
    {
        int cost = 0;
        switch (unitType)
        {
            case UnitType.ABA:
                cost = abaUnitCost;
                break;
            case UnitType.SNRK2:
                cost = snark2UnitCost;
                break;
            default:
                Debug.Log("Unable to find cost for unit type: " + unitType);
                break;
        }
        return cost;
    }

    public int GetMaxUnitHealth(UnitType unitType)
    {
        switch(unitType)
        {
            case UnitType.ABA:
                return abaUnitMaxHealth;
            case UnitType.BASIC_ENEMY:
                return enemyUnitMaxHealth;
            case UnitType.SNRK2:
                return snrkUnitMaxHealth;
        }
        return 0;
    }

    public int GetTowerCost(StructureType structureType)
    {
        int cost = 0;
        switch(structureType)
        {
            case StructureType.ABA_TOWER:
                cost = abaTowerCost;
                break;
            case StructureType.PPC2_TOWER:
                cost = ppc2TowerCost;
                break;
            case StructureType.CHLOROPLAST:
                cost = chloroplastTowerCost;
                break;
            case StructureType.MITOCHONDRIA:
                cost = mitoTowerCost;
                break;
            default:
                Debug.Log("Unable to find structure type:"  + structureType.ToString());
                break;
        }
        return cost;
    }
}
}