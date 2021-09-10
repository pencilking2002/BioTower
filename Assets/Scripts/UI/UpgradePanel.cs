using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BioTower.SaveData;
using UnityEngine.SceneManagement;

namespace BioTower
{
public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private Image[] upgradeButtons;
    [SerializeField] private Color defaultButtonColor;
    public Image panel;
    public Image infoPanel;
    public Button chooseUpgradeButton;
    public Text upgradeDescription;


    public void Display(bool isAnimated=false)
    {
        panel.gameObject.SetActive(true);

        if (isAnimated)
        {
            var upgradePanelGo = panel.gameObject;
            upgradePanelGo.SetActive(true);

            float initLocalPosY = upgradePanelGo.transform.localPosition.y;
            upgradePanelGo.transform.localPosition = new Vector3(0, -500, 0);
            LeanTween.moveLocalY(upgradePanelGo, initLocalPosY, 0.25f).setEaseOutBack();

            infoPanel.gameObject.SetActive(false);
        }
    }

    public void Hide(bool isAnimated=false)
    {
        panel.gameObject.SetActive(false);
    }

    public void OnPressUpgradeButton01()
    {
        infoPanel.gameObject.SetActive(true);
    }

    public void OnPressUpgradeButton02()
    {
        infoPanel.gameObject.SetActive(true);
    }

    public void OnPressUpgradeButton03()
    {
        infoPanel.gameObject.SetActive(true);

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