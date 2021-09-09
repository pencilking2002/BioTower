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
    [SerializeField] private Color defaultButtonColor;
    public Image panel;
    [SerializeField] private Image upgradeButton_01;
    [SerializeField] private Image upgradeButton_02;
    [SerializeField] private Image upgradeButton_03;
    public Image infoPanel;
    public Button chooseUpgradeButton;
    public Text upgradeDescription;


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