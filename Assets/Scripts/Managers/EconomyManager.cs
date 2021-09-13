using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using BioTower.Units;
using BioTower.SaveData;

namespace BioTower
{
public class EconomyManager : MonoBehaviour
{
    public int playerCurrency;
    

    public void Init(LevelType levelType)
    {
        playerCurrency = Util.upgradeSettings.energy;
        EventManager.Game.onGainCurrency?.Invoke(0, playerCurrency);
        Debug.Log("Init economy");
    }

    public bool HasEnoughCurrency(int num)
    {
        return playerCurrency >= num;
    }

    public void SpendCurrency(int num)
    {
        playerCurrency -= num;
        if (playerCurrency < 0)
            playerCurrency = 0;
            
        EventManager.Game.onSpendCurrency?.Invoke(num, playerCurrency);
    }

    public void GainCurrency(int num)
    {
        Debug.Log($"Econ: GainCurrency. old player currency: {playerCurrency}. Amount: {num}. New player currency {playerCurrency + num}");
        playerCurrency += num;
        EventManager.Game.onGainCurrency?.Invoke(num, playerCurrency);    
    }

    public bool CanBuyTowerHeal()
    {
        return HasEnoughCurrency(GameSettings.healTowerCost);
    }

    public bool CanBuyTower(StructureType structureType)
    {
        var cost = Util.upgradeSettings.GetTowerCost(structureType);
        return HasEnoughCurrency(cost);
    }

    public void BuyTower(StructureType structureType)
    {
        int cost = Util.upgradeSettings.GetTowerCost(structureType);
        SpendCurrency(cost);   
    }

    public bool CanBuyUnit(UnitType unitType)
    {
        var unitCost = Util.upgradeSettings.GetUnitCost(unitType);
        return HasEnoughCurrency(unitCost);
    }

    public void BuyUnit(UnitType unitType)
    {
        var unitCost = Util.upgradeSettings.GetUnitCost(unitType);
        SpendCurrency(unitCost);
    }

    public void BuyTowerHeal()
    {
        SpendCurrency(GameSettings.healTowerCost);
    }

    // public void GainCrystalMoney()
    // {
    //     int amount = Util.gameSettings.crystalWorth;
    //     GainCurrency(amount);
    // }

    private void OnSnrk2UnitReachedBase(Snrk2Unit unit)
    {
        GainCurrency(GameSettings.crystalSnrk2Value);
    }

    private void OnCrystalTapped()
    {
        GainCurrency(GameSettings.crystalValue);
    }

    private void OnLightFragmentTapped()
    {
        GainCurrency(GameSettings.lightFragmentValue);
    }

    private void OnEnable()
    {
        EventManager.Game.onSnrk2UnitReachedBase += OnSnrk2UnitReachedBase;
        EventManager.Game.onCrystalTapped += OnCrystalTapped;
        EventManager.Game.onLightFragmentTapped += OnLightFragmentTapped;
    }

    private void OnDisable()
    {
        EventManager.Game.onSnrk2UnitReachedBase -= OnSnrk2UnitReachedBase;
        EventManager.Game.onCrystalTapped -= OnCrystalTapped;
        EventManager.Game.onLightFragmentTapped -= OnLightFragmentTapped;
    }

}
}