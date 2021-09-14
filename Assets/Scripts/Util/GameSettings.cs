using UnityEngine;
using BioTower.SaveData;
using System.Collections.Generic;
using System;

namespace BioTower
{
[CreateAssetMenu(fileName="GameSettings", menuName="GameSettings")]
public class GameSettings : ScriptableObject
{
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


            }
            return _upgradeLogicMap;
        }
    }

    public void SetUpgradeSettingsBasedOnGameData(GameData gameData)
    {
        upgradeSettings = gameData.settings;
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

    public void UpgradePpc2TowerDamage()
    {
        upgradeSettings.ppc2TowerDamage = 10;
    }
}   
}
