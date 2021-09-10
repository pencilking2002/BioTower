using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BioTower.SaveData;
using UnityEngine.SceneManagement;
using System;

namespace BioTower
{
public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private UpgradeButton selectedButton;
    [SerializeField] private UpgradeButton unlockUpgradeButton;
    [SerializeField] private UpgradeButton[] upgradeButtons;
    [SerializeField] private Color defaultButtonColor;
    public Image panel;
    public Image infoPanel;
    public Button chooseUpgradeButton;
    public Text upgradeDescription;


    private void AnimateUpgradePanel(bool slideIn, Action onComplete=null)
    {
        if (slideIn)
        {
            float initLocalPosY = panel.transform.localPosition.y;
            panel.transform.localPosition = new Vector3(0, -500, 0);
            LeanTween.moveLocalY(panel.gameObject, initLocalPosY, 0.25f)
            .setEaseOutBack()
            .setOnComplete(onComplete);
            infoPanel.gameObject.SetActive(false);
        }
        else
        {
            float targetLocalPosY = -500;
            LeanTween.moveLocalY(panel.gameObject, targetLocalPosY, 0.25f)
            .setEaseOutBack()
            .setOnComplete(onComplete);
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
        var upgrades = upgradeTree.GetUpgradesForLevel(currLevel);

        if (upgrades.isUnlock)
        {
            unlockUpgradeButton.gameObject.SetActive(true);
            unlockUpgradeButton.SetText(upgrades.unlockUpgrade.ToString());
            unlockUpgradeButton.SetUpgradeType(upgrades.unlockUpgrade);

            foreach(var btn in upgradeButtons)
                btn.gameObject.SetActive(false);

        }
        else
        {
            unlockUpgradeButton.gameObject.SetActive(false);
            foreach(var btn in upgradeButtons)
            {
                btn.gameObject.SetActive(true);
            }
            upgradeButtons[0].SetText(upgrades.upgrade_01.ToString());
            upgradeButtons[0].SetUpgradeType(upgrades.upgrade_01);

            upgradeButtons[1].SetText(upgrades.upgrade_02.ToString());
            upgradeButtons[1].SetUpgradeType(upgrades.upgrade_02);

            upgradeButtons[2].SetText(upgrades.upgrade_03.ToString());
            upgradeButtons[2].SetUpgradeType(upgrades.upgrade_03);
        }
    }

    public void OnPressUpgradeButton01()
    {
        infoPanel.gameObject.SetActive(true);
        selectedButton = upgradeButtons[0];
    }

    public void OnPressUpgradeButton02()
    {
        infoPanel.gameObject.SetActive(true);
        selectedButton = upgradeButtons[1];
    }

    public void OnPressUpgradeButton03()
    {
        infoPanel.gameObject.SetActive(true);
        selectedButton = upgradeButtons[2];
    }

    public void OnPressPurchaseUpgradeButton()
    {
        var gameData = GameManager.Instance.saveManager.Load();
        var currLevel = (int) LevelInfo.current.levelType;
        gameData.currLevel = ++currLevel;
        GameManager.Instance.saveManager.Save(gameData);
        BootController.levelToLoadInstantly = gameData.currLevel;
        SceneManager.LoadScene(0);
    }
}
}