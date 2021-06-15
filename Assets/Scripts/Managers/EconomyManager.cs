using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{
public class EconomyManager : MonoBehaviour
{
    public int startingCurrency = 100;
    public int playerCurrency;
    
    
    // [Header("Structure Price List")]
    // public int abaTowerCost;
    // public int ppc2TowerCost;
    // public int chloroplastTowerCost;


    // [Header("Unit Price List")]
    // public int abaUnitCost;

    // [Header("Gain Money List")]
    // public int crystalWorth;

    //[Header("Misc")]

    
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

    private bool CanBuyAbaTower()
    {
        return HasEnoughCurrency(GameManager.Instance.gameSettings.abaTowerCost);
    }

    private bool CanBuyPPC2Tower()
    {
        return HasEnoughCurrency(GameManager.Instance.gameSettings.ppc2TowerCost);
    }

    private bool CanBuyChloroplastTower()
    {
        return HasEnoughCurrency(GameManager.Instance.gameSettings.chloroplastTowerCost);
    }

    public bool CanBuyTowerHeal()
    {
        return HasEnoughCurrency(GameManager.Instance.gameSettings.healTowerCost);
    }

    public bool CanBuyTower(StructureType structureType)
    {
        switch (structureType)
        {
            case StructureType.ABA_TOWER:
                return CanBuyAbaTower();
            case StructureType.PPC2_TOWER:
                return CanBuyPPC2Tower();
             case StructureType.CHLOROPLAST:
                return CanBuyChloroplastTower();
            default:
                Debug.Log("Structure Type not recognized: " + structureType.ToString());
                break;
        }
        return false;
    }

    public void BuyTower(StructureType structureType)
    {
        switch (structureType)
        {
            case StructureType.ABA_TOWER:
                BuyAbaTower();
                break;
            case StructureType.PPC2_TOWER:
                BuyPPC2Tower();
                break;
             case StructureType.CHLOROPLAST:
                BuyChloroplastTower();
                break;
        }
    }

    public void BuyAbaTower()
    {
        SpendCurrency(GameManager.Instance.gameSettings.abaTowerCost);
    }

    public void BuyPPC2Tower()
    {
        SpendCurrency(GameManager.Instance.gameSettings.ppc2TowerCost);
    }

    public void BuyChloroplastTower()
    {
        SpendCurrency(GameManager.Instance.gameSettings.chloroplastTowerCost);
    }

    public bool CanBuyAbaUnit()
    {
        return HasEnoughCurrency(GameManager.Instance.gameSettings.abaUnitCost);
    }

    public void BuyAbaUnit()
    {
        SpendCurrency(GameManager.Instance.gameSettings.abaUnitCost);
    }

    public void GainCrystalMoney()
    {
        int amount = GameManager.Instance.gameSettings.crystalWorth;
        GainCurrency(amount);
    }

}
}