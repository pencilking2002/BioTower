﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using Sirenix.OdinInspector;
using BioTower.Structures;

namespace BioTower
{
[CreateAssetMenu(fileName="GameSettings", menuName="GameSettings")]
public class GameSettings : ScriptableObject
{
    [Range(1,10)]public float timeScale = 1.0f;

    [Header("Unit Damage")]
    [TabGroup("Units")] public int ppc2Damage = 5;
    [TabGroup("Units")] public int abaDamage = 5;
    [TabGroup("Units")] public int basicEnemyDamage = 5;


    [Header("Unit Health")]
    [TabGroup("Units")] public int abaUnitMaxHealth = 10;
    [TabGroup("Units")] public int enemyUnitMaxHealth = 10;
    [TabGroup("Units")] public int abaUnitCost;


    [Header("Tower Health")]
    [TabGroup("Towers")] public bool enableTowerHealthDecline;


    [Header("Tower Cost")]
    [TabGroup("Towers")] [Range(0,100)] public int abaTowerCost;
    [TabGroup("Towers")] public int ppc2TowerCost;
    [TabGroup("Towers")] public int chloroplastTowerCost;
    [TabGroup("Towers")] public int mitoTowerCost;
    [TabGroup("Towers")] public int healTowerCost;
    [TabGroup("Towers")] public int healTowerAmount;

    [TabGroup("Misc")] public int crystalWorth;


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

    public int GetMaxUnitHealth(UnitType unitType)
    {
        switch(unitType)
        {
            case UnitType.ABA:
                return abaUnitMaxHealth;
            case UnitType.BASIC_ENEMY:
                return enemyUnitMaxHealth;
        }

        Debug.Log("Unit type not recognized: " + unitType);
        return 0;
    }
}
}
