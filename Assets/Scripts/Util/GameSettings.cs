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
        upgradeSettings.abaMaxInfluenceRadius = 3000;
        upgradeSettings.abaMapScale = 3000;
        upgradeSettings.abaInfluenceShapeRadius = 3000;
    }

    public void UpgradeAbaTowerUnitCost()
    {
        upgradeSettings.abaUnitCost = 3;
    } 

    public void UpgradeAbaTowerRandomHeal()
    {
        upgradeSettings.enableAbaTowerRandomHeal = true;
    }  

    public void UpgradeAbaUnitHealth()
    {
        upgradeSettings.abaUnitMaxHealth = 15;
    }

    public void UpgradeAbaUnitDamage()
    {
        upgradeSettings.abaDamage = 10;
    }


}   
}
