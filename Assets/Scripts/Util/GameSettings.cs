using System.Collections;
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

    [Header("Units")]
    [TabGroup("Units")] public int ppc2Damage = 5;
    [TabGroup("Units")] public int abaDamage = 5;
    [TabGroup("Units")] public int basicEnemyDamage = 5;
    [TabGroup("Units")] public int abaUnitMaxHealth = 10;
    [TabGroup("Units")] public int snrkUnitMaxHealth = 5;
    [TabGroup("Units")] public int enemyUnitMaxHealth = 10;
    [TabGroup("Units")] public int abaUnitCost;
    [TabGroup("Units")] public int snark2UnitCost;


    [Header("Tower Health")]
    [TabGroup("Towers")] public bool enableTowerHealthDecline;


    [Header("Tower Cost")]
    [TabGroup("Towers")] public int ppc2TowerCost;
    [TabGroup("Towers")] public int chloroplastTowerCost;
    [TabGroup("Towers")] public int mitoTowerCost;
    [TabGroup("Towers")] public int healTowerCost;
    [TabGroup("Towers")] public int spawnLightDropCost;
    [TabGroup("Towers")] public int healTowerAmount;

    [Header("ABA Tower")]
    [TabGroup("Towers")] [Range(0,100)] public int abaTowerCost;
    [TabGroup("Towers")] public int abaUnitSpawnLimit;
    [TabGroup("Towers")] public float abaMaxInfluenceRadius;
    [TabGroup("Towers")] public float abaMapScale;
    [TabGroup("Towers")] public float abaInfluenceShapeRadius;


    [TabGroup("Misc")] public int lightFragmentWorth;
    [TabGroup("Misc")] public int crystalWorth;
    [TabGroup("Misc")] public int crystalSnrk2Worth;
    [TabGroup("Misc")] public int startingEnergy;
    [TabGroup("Misc")] public int energy;


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

        Debug.Log("Unit type not recognized: " + unitType);
        return 0;
    }
}
}
