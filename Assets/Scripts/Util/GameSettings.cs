using UnityEngine;
using BioTower.SaveData;
using System.Collections.Generic;
using System;
using BioTower.Units;
using System.Reflection;

namespace BioTower
{
[CreateAssetMenu(fileName="GameSettings", menuName="GameSettings")]
public class GameSettings : ScriptableObject
{
    public int basicEnemyDamage = 5;        // N/A
    public int enemyUnitMaxHealth = 10;     // N/A
    public int snark2UnitCost = 3;          // N/A
    [Range(0,1)] public float randomHealChance = 0.1f;
    public int declineDamage = 1;
    public int randomHealAmount = 2;

    [Space(10)]
    public Params defaultSettings;
    public Params upgradeSettings;

   


    private Dictionary<UpgradeType, Action> _upgradeLogicMap;

    public Dictionary<UpgradeType, Action> upgradeLogicMap
    {
        get 
        {
            if (_upgradeLogicMap == null)
            {
                _upgradeLogicMap = new Dictionary<UpgradeType, Action>();
                _upgradeLogicMap.Add(UpgradeType.ABA_TOWER_INFLUENCE, UpgradeAbaTowerInfluence);
                _upgradeLogicMap.Add(UpgradeType.ABA_TOWER_UNIT_COST, UpgradeAbaTowerUnitCost);
                _upgradeLogicMap.Add(UpgradeType.ABA_TOWER_RANDOM_HEAL, UpgradeAbaTowerRandomHeal);
                _upgradeLogicMap.Add(UpgradeType.ABA_UNIT_HEALTH, UpgradeAbaUnitHealth);
                _upgradeLogicMap.Add(UpgradeType.ABA_UNIT_DAMAGE, UpgradeAbaUnitDamage);
                _upgradeLogicMap.Add(UpgradeType.ABA_UNIT_SPEED, UpgradeAbaUnitSpeed);
                _upgradeLogicMap.Add(UpgradeType.ABA_UNIT_SPAWN_LIMIT, UpgradeAbaSpawnLimit);

                _upgradeLogicMap.Add(UpgradeType.PP2C_TOWER_UNLOCK, UnlockPPC2Tower);
                _upgradeLogicMap.Add(UpgradeType.PP2C_TOWER_INFLUENCE, UpgradePPC2TowerInfluence);
                _upgradeLogicMap.Add(UpgradeType.PP2C_TOWER_FIRE_RATE, UpgradePpc2FireRate);
                _upgradeLogicMap.Add(UpgradeType.PP2C_EXPLOSION_RADIUS, UpgradePpc2ExplosionRadius);
                _upgradeLogicMap.Add(UpgradeType.PP2C_TOWER_DAMAGE, UpgradePpc2TowerDamage);
                _upgradeLogicMap.Add(UpgradeType.PP2C_TOWER_RANDOM_HEAL, UpgradePpc2TowerRandomHeal);
                _upgradeLogicMap.Add(UpgradeType.MITO_TOWER_UNLOCK, UnlockMitoTower);
                _upgradeLogicMap.Add(UpgradeType.MITO_TOWER_ENERGY, UpgradeMitoEnergy);
                _upgradeLogicMap.Add(UpgradeType.MITO_TOWER_COOLDOWN, UpgradeMitoTowerCooldown);
                _upgradeLogicMap.Add(UpgradeType.MITO_TOWER_HEALTH, UpgradeMitoHealth);

                _upgradeLogicMap.Add(UpgradeType.CHLORO_TOWER_UNLOCK, UnlockChloroTower);
                _upgradeLogicMap.Add(UpgradeType.CHLORO_TOWER_ENERGY, UpgradeChloroTowerEnergy);
                _upgradeLogicMap.Add(UpgradeType.CHLORO_TOWER_ENERGY_RATE, UpgradeChloroTowerEnergyRate);
                _upgradeLogicMap.Add(UpgradeType.CHLORO_TOWER_LIGHT_CLICK, UpgradeChloroLightTap);
                _upgradeLogicMap.Add(UpgradeType.CHLORO_TOWER_RANDOM_HEAL, UpgradeChloroTowerRandomHeal);

                _upgradeLogicMap.Add(UpgradeType.SNRK2_UNIT_UNLOCK, UnlockSnrk2Unit);
                _upgradeLogicMap.Add(UpgradeType.SNRK2_SPEED, UpgradeSnrk2UnitSpeed);
                _upgradeLogicMap.Add(UpgradeType.SNRK2_UNIT_HEALTH, UpgradeSnk2UnitHealth);
                _upgradeLogicMap.Add(UpgradeType.SNRK2_CRYSTAL_VALUE, UpgradeSnrk2CrystalValue);

                _upgradeLogicMap.Add(UpgradeType.PLAYER_TOWER_HEALTH, UpgradePlayerTowerHealth);
                _upgradeLogicMap.Add(UpgradeType.PLAYER_TOWER_HEAL, UpgradePlayerTowerHealing);


            }
            return _upgradeLogicMap;
        }
    }
    public int GetMaxUnitHealth(UnitType unitType)
    {
        switch(unitType)
        {
            case UnitType.ABA:
                return upgradeSettings.abaUnitMaxHealth;
            case UnitType.BASIC_ENEMY:
                return enemyUnitMaxHealth;
            case UnitType.SNRK2:
                return upgradeSettings.snrkUnitMaxHealth;
        }
        return 0;
    }

    public int GetUnitCost(UnitType unitType)
    {
        int cost = 0;
        switch (unitType)
        {
            case UnitType.ABA:
                cost = upgradeSettings.abaUnitCost;
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

    public void SetUpgradeSettingsBasedOnGameData(GameData gameData)
    {
        upgradeSettings = gameData.settings;
    }

    public void SetUpgradeSettingsToDefault()
    {
        object defaultSerializedObject = defaultSettings;
        Type serializedObjectType = defaultSettings.GetType();
        FieldInfo[] defaultFields = serializedObjectType.GetFields();

        object upgradeSerializedObject = upgradeSettings;
        FieldInfo[] upgradeFields = serializedObjectType.GetFields();
        
        for(int i=0; i<defaultFields.Length; i++)
        {
            object _fieldValue = defaultFields[i].GetValue(defaultSettings);
            string _fieldName = defaultFields[i].Name;

            if (_fieldValue != null)
            {
                upgradeFields[i].SetValue(upgradeSerializedObject, _fieldValue);
            }
        }
    }
    
    public void UpgradeAbaTowerInfluence()
    {
        upgradeSettings.abaMaxInfluenceRadius_float = 3000;
        upgradeSettings.abaMapScale_float = 3000;
        upgradeSettings.abaInfluenceShapeRadius_float = 3000;
    }

    // ABA Tower upgrades -----------------------------------------------
    public void UpgradeAbaTowerUnitCost() { upgradeSettings.abaUnitCost = 3; } 
    public void UpgradeAbaTowerRandomHeal() { upgradeSettings.enableAbaTowerRandomHeal = true; }  
    public void UpgradeAbaUnitHealth() { upgradeSettings.abaUnitMaxHealth = 15; }
    public void UpgradeAbaUnitDamage() { upgradeSettings.abaUnitDamage = 10; }
    public void UpgradeAbaUnitSpeed() { upgradeSettings.abaUnitMaxSpeed_float = 700; }
    public void UpgradeAbaSpawnLimit() { upgradeSettings.abaUnitSpawnLimit = 5; }


    // PPC2 Tower upgrades -----------------------------------------------
    public void UnlockPPC2Tower() { upgradeSettings.ppc2TowerUnlocked = true; }
    public void UpgradePPC2TowerInfluence()
    {
        upgradeSettings.ppc2MaxInfluenceRadius_float = 3000;
        upgradeSettings.ppc2MapScale_float = 3000;
        upgradeSettings.ppc2InfluenceShapeRadius_float = 3000;
    }
    
    public void UpgradePpc2FireRate() { upgradeSettings.ppc2shootInterval_float = 700; }
    public void UpgradePpc2ExplosionRadius() 
    { 
        upgradeSettings.ppc2ExplosionColliderRadius_float = 1400; 
        upgradeSettings.ppc2ExplosionSpriteRadius_float = 800; 
    }

    public void UpgradePpc2TowerDamage() { upgradeSettings.ppc2TowerDamage = 10; }
    public void UpgradePpc2TowerRandomHeal() { upgradeSettings.ppc2TowerDamage = 10; }

    // Mito Tower upgrades -----------------------------------------------
    public void UnlockMitoTower() { upgradeSettings.mitoTowerUnlocked = true; }
    public void UpgradeMitoEnergy() { upgradeSettings.lightFragmentValue = 10; }
    public void UpgradeMitoTowerCooldown() { upgradeSettings.mitoShootInterval_float = 1000; }
    public void UpgradeMitoHealth() { upgradeSettings.mitoTowerMaxHealth = 15; }


    // Chloro Tower upgrade
    public void UnlockChloroTower() { upgradeSettings.chloroTowerUnlocked = true; }
    public void UpgradeChloroTowerEnergy() { upgradeSettings.lightFragmentValue = 15; }
    public void UpgradeChloroTowerEnergyRate() { upgradeSettings.chloroShootInterval_float = 3500; }
    public void UpgradeChloroLightTap() { upgradeSettings.numFragmentsPickedUpOnTap = 3; }
    public void UpgradeChloroTowerRandomHeal() { upgradeSettings.enableChloroTowerRandomHeal = true; }

    // Snrk2 
    public void UnlockSnrk2Unit() { upgradeSettings.snrk2UnitUnlocked = true; }
    public void UpgradeSnrk2UnitSpeed() { upgradeSettings.snrk2UnitSpeed_float = 1500; }
    public void UpgradeSnk2UnitHealth() { upgradeSettings.snrkUnitMaxHealth = 10; }
    public void UpgradeSnrk2CrystalValue() { upgradeSettings.crystalSnrk2Value = 30; }

    // Player tower
    public void UpgradePlayerTowerHealth() { upgradeSettings.playerTowerMaxhealth = 30; }
    public void UpgradePlayerTowerHeal() { upgradeSettings.playerTowerMaxhealth = 30; }
    public void UpgradePlayerTowerHealing() { upgradeSettings.enablePlayerTowerHealing = true; }
}   
}
