using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BioTower.SaveData;
using BioTower.Structures;
using BioTower.Units;

namespace BioTower
{
[CreateAssetMenu(fileName="UpgradeSettings", menuName="UpgradeSettings")]
[TypeInfoBox("Gets updated at the beginning of every level by the Save System and used by Towers and units to get data about how to behave based on the current upgrades")]
public class UpgradeSettings : ScriptableObject
{

    [Header("Units")]
    [TabGroup("Units")] public int ppc2Damage = 5;
    [TabGroup("Units")] public int abaDamage = 5;
    [TabGroup("Units")] public int basicEnemyDamage = 5;
    [TabGroup("Units")] public int abaUnitMaxHealth = 10;
    [TabGroup("Units")] public int snrkUnitMaxHealth = 5;
    [TabGroup("Units")] public int enemyUnitMaxHealth = 10;
    [TabGroup("Units")] public int abaUnitCost;
    [TabGroup("Units")] public int snark2UnitCost;


    [Header("ABA Tower")]
    [TabGroup("Towers")][Range(0,100)] public int abaTowerCost;
    [TabGroup("Towers")] public int abaUnitSpawnLimit;
    [TabGroup("Towers")] public float abaMaxInfluenceRadius;
    [TabGroup("Towers")] public float abaMapScale;
    [TabGroup("Towers")] public float abaInfluenceShapeRadius;

    
    [Header("Tower Cost")]
    [TabGroup("Towers")] public int ppc2TowerCost;
    [TabGroup("Towers")] public int chloroplastTowerCost;
    [TabGroup("Towers")] public int mitoTowerCost;
    [TabGroup("Towers")] public int healTowerCost;
    [TabGroup("Towers")] public int spawnLightDropCost;
    [TabGroup("Towers")] public int healTowerAmount;

    [TabGroup("Misc")] public int energy = 80;

    public void SetData(GameData gameData)
    {
        // ABA tower influence
        abaUnitSpawnLimit = gameData.abaTowerSettings.abaUnitSpawnLimit;
        abaMaxInfluenceRadius = gameData.abaTowerSettings.abaMaxInfluenceRadius / 1000;
        abaMapScale = gameData.abaTowerSettings.abaMapScale / 1000;
        abaInfluenceShapeRadius = gameData.abaTowerSettings.abaInfluenceShapeRadius / 1000;

    }

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

        //Debug.Log("Unit type not recognized: " + unitType);
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