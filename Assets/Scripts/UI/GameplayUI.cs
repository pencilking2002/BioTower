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

    [SerializeField] private Button AbaTowerButton;
    [SerializeField] private Button Pp2cTowerButton;
    [SerializeField] private Button chloroplastTowerButton;

    [SerializeField] private TextMeshProUGUI playerCurrencyText;
    private Dictionary<StructureType,Button> towerButtonMap = new Dictionary<StructureType, Button>();

    private void Awake()
    {
        towerButtonMap.Add(StructureType.ABA_TOWER, AbaTowerButton);
        towerButtonMap.Add(StructureType.PPC2_TOWER, Pp2cTowerButton);
        towerButtonMap.Add(StructureType.CHLOROPLAST, chloroplastTowerButton);
    }

    private void Update()
    {
        playerCurrencyText.text = GameManager.Instance.econManager.playerCurrency.ToString();
    }


    // BUTTON METHODS -------------------------------------------------------------------------------
    
    public void OnPressAbaTowerButton()
    {
        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.ABA_TOWER];
        if (canBuildTower)
            EventManager.UI.onPressTowerButton?.Invoke(StructureType.ABA_TOWER);
    }
    
    public void OnPressPPC2TowerButton()
    {
        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.PPC2_TOWER];
        if (canBuildTower)
            EventManager.UI.onPressTowerButton?.Invoke(StructureType.PPC2_TOWER);
    }

    public void OnPressChloroplastButton()
    {
        bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.PPC2_TOWER];
        if (canBuildTower)
            EventManager.UI.onPressTowerButton?.Invoke(StructureType.CHLOROPLAST);
    }

    private void OnSpendCurrency(int numSpent, int currTotal)
    {
        //playerCurrencyText.text = currTotal.ToString();
    }

    private void OnGainCurrency(int numGained, int currTotal)
    {
        //playerCurrencyText.text = currTotal.ToString();
        Debug.Log($"Currency gained");
    }

    private void OnStructureCooldownStarted(StructureType structureType, float cooldown)
    {
        if (!towerButtonMap.ContainsKey(structureType))
            return;
            
        var button = towerButtonMap[structureType];

        button.interactable = false;
        var cooldownImage = button.transform.Find("Cooldown").GetComponent<Image>();
        LeanTween.value(1, 0, cooldown).setOnUpdate((float val) => {
            cooldownImage.fillAmount = val;
        })
        .setOnComplete(() => {
            button.interactable = true;
        });
    }   

    private void OnEnable()
    {
        EventManager.Game.onSpendCurrency += OnSpendCurrency;
        EventManager.Game.onGainCurrency += OnGainCurrency;
        EventManager.Structures.onStructureCooldownStarted += OnStructureCooldownStarted;
    }

    private void OnDisable()
    {
        EventManager.Game.onSpendCurrency -= OnSpendCurrency;
        EventManager.Game.onGainCurrency -= OnGainCurrency;
        EventManager.Structures.onStructureCooldownStarted -= OnStructureCooldownStarted;
    }
}
}
