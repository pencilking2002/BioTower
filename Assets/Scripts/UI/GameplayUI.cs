using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using BioTower.Structures;
using TMPro;

namespace BioTower.UI
{
public class GameplayUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup gameUIPanel;
    [SerializeField] private Button spawnAbaTowerButton;
    [SerializeField] private TextMeshProUGUI playerCurrencyText;

    public void OnPressAbaTowerButton()
    {
        EventManager.UI.onPressTowerButton?.Invoke(StructureType.ABA_TOWER);
    }
    
    public void OnPressPPC2TowerButton()
    {
        EventManager.UI.onPressTowerButton?.Invoke(StructureType.PPC2_TOWER);
    }

    private void OnSpendCurrency(int numSpent, int currTotal)
    {
        playerCurrencyText.text = currTotal.ToString();
    }

    private void OnGainCurrency(int numGained, int currTotal)
    {
        playerCurrencyText.text = currTotal.ToString();
    }
    

    private void OnEnable()
    {
        EventManager.Game.onSpendCurrency += OnSpendCurrency;
        EventManager.Game.onGainCurrency += OnGainCurrency;
    }

    private void OnDisable()
    {
        EventManager.Game.onSpendCurrency -= OnSpendCurrency;
        EventManager.Game.onGainCurrency -= OnGainCurrency;
    }
}
}
