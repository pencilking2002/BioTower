using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BioTower.SaveData;
using UnityEngine.SceneManagement;
using System;
using Sirenix.OdinInspector;

namespace BioTower
{
public class UpgradePanel : MonoBehaviour
{
    [ReadOnly][SerializeField] private UpgradeButton selectedButton;
    [SerializeField] private UpgradeButton unlockUpgradeButton;
    [SerializeField] private UpgradeButton[] upgradeButtons;
    [SerializeField] private Color defaultButtonColor;
    public Image panel;
    public Image infoPanel;
    public Image itemImage;
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
        var upgradeTextData = GameManager.Instance.upgradeTextData;
        var upgrades = upgradeTree.GetUpgradesForLevel(currLevel);
        UpgradeData data = null;

        if (upgrades.isUnlock)
        {
            data = upgradeTextData.GetUpgradeTextData(upgrades.unlockUpgrade);
            unlockUpgradeButton.gameObject.SetActive(true);
            unlockUpgradeButton.SetText(data.buttonText);
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
            data = upgradeTextData.GetUpgradeTextData(upgrades.upgrade_01);
            upgradeButtons[0].SetText(data.buttonText);
            upgradeButtons[0].SetUpgradeType(upgrades.upgrade_01);
            upgradeButtons[0].SetIcon(data.sprite);

            data = upgradeTextData.GetUpgradeTextData(upgrades.upgrade_02);
            upgradeButtons[1].SetText(data.buttonText);
            upgradeButtons[1].SetUpgradeType(upgrades.upgrade_02);
            upgradeButtons[1].SetIcon(data.sprite);

            data = upgradeTextData.GetUpgradeTextData(upgrades.upgrade_03);
            upgradeButtons[2].SetText(data.buttonText);
            upgradeButtons[2].SetUpgradeType(upgrades.upgrade_03);
            upgradeButtons[2].SetIcon(data.sprite);
        }
    }

    public void OnPressUpgradeButton01()
    {
        infoPanel.gameObject.SetActive(true);
        selectedButton = upgradeButtons[0];

        var upgradeType = selectedButton.GetUpgradeType();
        var upgradeData = GameManager.Instance.upgradeTextData;
        var data = upgradeData.GetUpgradeTextData(upgradeType);
        upgradeDescription.text = data.descrptionText;
        itemImage.sprite = data.sprite;
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