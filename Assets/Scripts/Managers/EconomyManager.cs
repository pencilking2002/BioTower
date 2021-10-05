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
    

    public void Init(int currency)
    {
        //GainCurrency(currency);
        playerCurrency = currency;
        //playerCurrency = Util.gameSettings.upgradeSettings.energy;
        EventManager.Game.onGainCurrency?.Invoke(currency, playerCurrency);
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
        //Debug.Log($"Econ: GainCurrency. old player currency: {playerCurrency}. Amount: {num}. New player currency {playerCurrency + num}");
        playerCurrency += num;
        EventManager.Game.onGainCurrency?.Invoke(num, playerCurrency);    
    }

    public bool CanBuyTowerHeal()
    {
        return HasEnoughCurrency(Util.upgradeSettings.healTowerCost);
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
        var unitCost = Util.gameSettings.GetUnitCost(unitType);
        return HasEnoughCurrency(unitCost);
    }

    public void BuyUnit(UnitType unitType)
    {
        var unitCost = Util.gameSettings.GetUnitCost(unitType);
        SpendCurrency(unitCost);
    }

    public void BuyTowerHeal()
    {
        SpendCurrency(Util.upgradeSettings.healTowerCost);
    }

    public bool CanBuyLightFragment()
    {
        var cost = Util.gameSettings.spawnLightDropCost;
        return HasEnoughCurrency(cost);
    }

    public void BuyLightFragment()
    {
        var cost = Util.gameSettings.spawnLightDropCost;
        SpendCurrency(cost);
    }

    private void OnSnrk2UnitReachedBase(Snrk2Unit unit)
    {
        GainCurrency(Util.upgradeSettings.crystalSnrk2Value);
    }

    private void OnLightFragmentTapped()
    {
        GainCurrency(Util.upgradeSettings.lightFragmentValue);
    }

    private void OnEnable()
    {
        EventManager.Game.onSnrk2UnitReachedBase += OnSnrk2UnitReachedBase;
        //EventManager.Game.onCrystalTapped += OnCrystalTapped;
        EventManager.Game.onLightFragmentTapped += OnLightFragmentTapped;
    }

    private void OnDisable()
    {
        EventManager.Game.onSnrk2UnitReachedBase -= OnSnrk2UnitReachedBase;
        //EventManager.Game.onCrystalTapped -= OnCrystalTapped;
        EventManager.Game.onLightFragmentTapped -= OnLightFragmentTapped;
    }

}
}