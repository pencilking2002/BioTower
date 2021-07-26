using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using BioTower.Units;

namespace BioTower
{
public class EconomyManager : MonoBehaviour
{
    public int startingCurrency = 100;
    public int playerCurrency;
    
    private void Start()
    {
       Init();
    }

    private void Init()
    {
        playerCurrency = startingCurrency;
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
        EventManager.Game.onSpendCurrency?.Invoke(num, playerCurrency);
    }

    private void GainCurrency(int num)
    {
        Debug.Log($"Econ: GainCurrency. old player currency: {playerCurrency}. Amount: {num}. New player currency {playerCurrency + num}");
        playerCurrency += num;
        EventManager.Game.onGainCurrency?.Invoke(num, playerCurrency);    
    }

    public bool CanBuyTowerHeal()
    {
        return HasEnoughCurrency(GameManager.Instance.gameSettings.healTowerCost);
    }

    public bool CanBuyTower(StructureType structureType)
    {
        var cost = Util.gameSettings.GetTowerCost(structureType);
        return HasEnoughCurrency(cost);
    }

    public void BuyTower(StructureType structureType)
    {
        int cost = Util.gameSettings.GetTowerCost(structureType);
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
        SpendCurrency(Util.gameSettings.healTowerCost);
    }

    public void GainCrystalMoney()
    {
        int amount = Util.gameSettings.crystalWorth;
        GainCurrency(amount);
    }

}
}