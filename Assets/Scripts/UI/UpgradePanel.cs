﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BioTower.SaveData;
using UnityEngine.SceneManagement;
using System;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace BioTower
{
public class UpgradePanel : MonoBehaviour
{
    [ReadOnly][SerializeField] private UpgradeButton selectedButton;
    [SerializeField] private UpgradeButton unlockUpgradeButton;
    [SerializeField] private UpgradeButton[] upgradeButtons;
    [SerializeField] private Color defaultButtonColor;
    public RectTransform panel;
    public Image infoPanel;
    public Image itemImage;
    public Button chooseUpgradeButton;
    public TextMeshProUGUI upgradeDescription;


    private void Awake()
    {
        panel.gameObject.SetActive(false);
    }

    private void AnimateUpgradePanel(bool slideIn, Action onComplete=null)
    {
        if (slideIn)
        {
            panel.gameObject.SetActive(true);
            float initLocalPosY = panel.transform.localPosition.y;
            panel.transform.localPosition = new Vector3(0, -500, 0);

            var seq = LeanTween.sequence();
            seq.append(LeanTween.moveLocalY(panel.gameObject, initLocalPosY, 0.25f)
                .setEaseOutBack()
                .setIgnoreTimeScale(true)
            );

            seq.append(() => {
                EventSystem.current.SetSelectedGameObject(upgradeButtons[0].gameObject, null);  
                OnPressUpgradeButton01(false);
            });

            seq.append (gameObject, onComplete);
        }
        else
        {
            float targetLocalPosY = -500;
            LeanTween.moveLocalY(panel.gameObject, targetLocalPosY, 0.25f)
            .setEaseOutBack()
            .setOnComplete(onComplete)
            .setIgnoreTimeScale(true);
        }
    }

    public void Display(bool isAnimated=false)
    {
        panel.gameObject.SetActive(true);
        SetupUpdgradeButtons();

        if (isAnimated)
        {
            AnimateUpgradePanel(true);
        }
    }

    public void Hide(bool isAnimated=false)
    {
        if (isAnimated)
        {
            AnimateUpgradePanel(false, () => {
                panel.gameObject.SetActive(false);
            });
        }
    }

    private void SetupUpdgradeButtons()
    {
        var currLevel = LevelInfo.current.levelType;
        
        // Get the upgrades for this level
        var upgradeTree = GameManager.Instance.upgradeTree;
        var upgradeTextData = GameManager.Instance.upgradeTextData;
        var upgrades = upgradeTree.GetUpgradesForLevel(currLevel);
        UpgradeData data = null;

        unlockUpgradeButton.gameObject.SetActive(upgrades.isUnlock);

        if (upgrades.isUnlock)
        {
            data = upgradeTextData.GetUpgradeTextData(upgrades.unlockUpgrade);
            unlockUpgradeButton.gameObject.SetActive(true);
            unlockUpgradeButton.SetText(data.buttonText);
            unlockUpgradeButton.SetUpgradeType(upgrades.unlockUpgrade);

            for (int i=0; i<upgradeButtons.Length; i++)
                upgradeButtons[i].gameObject.SetActive(false);
            
        }
        else
        {
            for (int i=0; i<upgradeButtons.Length; i++)
            {
                UpgradeButton btn = upgradeButtons[i];
                UpgradeType upgradeType = UpgradeType.NONE;

                if (i==0) upgradeType = upgrades.upgrade_01;
                else if (i==1) upgradeType = upgrades.upgrade_02;
                else if (i==2) upgradeType = upgrades.upgrade_03;    
                
                data = upgradeTextData.GetUpgradeTextData(upgradeType);
                btn.SetUpgradeType(upgradeType);
                btn.SetText(data.buttonText);
                btn.SetIcon(data.sprite);
                btn.gameObject.SetActive(true);
            }

        }
    }

    public void OnPressUnlockUpgradeButton()
    {
        infoPanel.gameObject.SetActive(true);
        selectedButton = unlockUpgradeButton;

        var upgradeType = selectedButton.GetUpgradeType();
        var upgradeData = GameManager.Instance.upgradeTextData;
        var data = upgradeData.GetUpgradeTextData(upgradeType);
        upgradeDescription.text = data.descrptionText;
        itemImage.sprite = data.sprite;
    }

    public void OnPressUpgradeButton01(bool useSound=true)
    {
        infoPanel.gameObject.SetActive(true);
        selectedButton = upgradeButtons[0];

        var upgradeType = selectedButton.GetUpgradeType();
        var upgradeData = GameManager.Instance.upgradeTextData;
        var data = upgradeData.GetUpgradeTextData(upgradeType);
        upgradeDescription.text = data.descrptionText;
        itemImage.sprite = data.sprite;
        
        if(useSound)
            EventManager.UI.onTapButton?.Invoke(true);
    }

    public void OnPressUpgradeButton02()
    {
        infoPanel.gameObject.SetActive(true);
        selectedButton = upgradeButtons[1];

        var upgradeType = selectedButton.GetUpgradeType();
        var upgradeData = GameManager.Instance.upgradeTextData;
        var data = upgradeData.GetUpgradeTextData(upgradeType);
        upgradeDescription.text = data.descrptionText;
        itemImage.sprite = data.sprite;
        EventManager.UI.onTapButton?.Invoke(true);
    }

    public void OnPressUpgradeButton03()
    {
        infoPanel.gameObject.SetActive(true);
        selectedButton = upgradeButtons[2];

        var upgradeType = selectedButton.GetUpgradeType();
        var upgradeData = GameManager.Instance.upgradeTextData;
        var data = upgradeData.GetUpgradeTextData(upgradeType);
        upgradeDescription.text = data.descrptionText;
        itemImage.sprite = data.sprite;
        EventManager.UI.onTapButton?.Invoke(true);
    }

    public void OnPressPurchaseUpgradeButton()
    {
        var upgradeType = selectedButton.GetUpgradeType();
        var levelType = LevelInfo.current.levelType;
        var levelUpgrades = Util.upgradeTree.GetUpgradesForLevel(levelType);
        var upgradeVarIndex = Util.upgradeTree.GetUpgradeVarName(levelUpgrades, upgradeType);
        var gameData = GameManager.Instance.saveManager.Load();
        var currLevel = (int) levelType;
        var chosenUpgrade = new ChosenUpgrade(currLevel, upgradeVarIndex);
        bool hasUpgrade = false;

        // Check if an upgrade the current level already exists, overwrite if it does
        foreach(ChosenUpgrade upgrade in gameData.chosenUpgrades)
        {
            if (upgrade.level == currLevel)
            {
                hasUpgrade = true;
                upgrade.varIndex = upgradeVarIndex;
            }
        }

        // Otherwise if it doesn't add it to the list
        if (!hasUpgrade)
            gameData.chosenUpgrades.Add(chosenUpgrade);
        
        Util.gameSettings.upgradeSettings = gameData.settings;
        Util.gameSettings.upgradeLogicMap[upgradeType]();
        gameData.settings = Util.gameSettings.upgradeSettings;

        gameData.settings.currLevel = ++currLevel;
        GameManager.Instance.saveManager.Save(gameData);
        BootController.levelToLoadInstantly = gameData.settings.currLevel;
        EventManager.UI.onTapButton?.Invoke(true);

        LeanTween.delayedCall(1.0f, () => {
            SceneManager.LoadScene(0);
        }).setIgnoreTimeScale(true);
    }
}
}