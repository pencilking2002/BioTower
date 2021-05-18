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

    
    [Header("Structure Price List")]
    public int abaTowerCost;
    public int ppc2TowerCost;


    [Header("Unit Price List")]
    public int abaUnitCost;
    
    private void Start()
    {
        playerCurrency = startingCurrency;
        EventManager.Game.onGainCurrency?.Invoke(0, playerCurrency);
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

    public void AddCurrency(int num)
    {
        playerCurrency += num;
        EventManager.Game.onGainCurrency?.Invoke(num, playerCurrency);
    }

    private bool CanBuyAbaTower()
    {
        return HasEnoughCurrency(abaTowerCost);
    }

    private bool CanBuyPPC2Tower()
    {
        return HasEnoughCurrency(ppc2TowerCost);
    }

    public bool CanBuyTower(StructureType structureType)
    {
        switch (structureType)
        {
            case StructureType.ABA_TOWER:
                return CanBuyAbaTower();
            case StructureType.PPC2_TOWER:
                return CanBuyPPC2Tower();
            default:
                Debug.Log("Structure Type not recognized: " + structureType.ToString());
                break;
        }
        return false;
    }

    public void BuyAbaTower()
    {
        SpendCurrency(abaTowerCost);
    }

     public void BuyPPC2Tower()
    {
        SpendCurrency(ppc2TowerCost);
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
        }
    }

    public bool CanBuyAbaUnit()
    {
        return HasEnoughCurrency(abaUnitCost);
    }

    public void BuyAbaUnit()
    {
        SpendCurrency(abaUnitCost);
    }

}
}