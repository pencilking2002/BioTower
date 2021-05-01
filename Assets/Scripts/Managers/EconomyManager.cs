using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class EconomyManager : MonoBehaviour
{
    public int startingCurrency = 100;
    public int playerCurrency;

    [Header("Price list")]
    public int abaTowerCost;
    public int abaUnitCost;
    
    private void Awake()
    {
        playerCurrency = startingCurrency;
    }

    public bool HasEnoughCurrency(int num)
    {
        return playerCurrency >= num;
    }

    public void SpendCurrency(int num)
    {
        playerCurrency -= num;
    }

    public void AddCurrency(int num)
    {
        playerCurrency += num;
    }

    public bool CanBuyAbaTower()
    {
        return HasEnoughCurrency(abaTowerCost);
    }

    public void BuyAbaTower()
    {
        SpendCurrency(abaTowerCost);
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