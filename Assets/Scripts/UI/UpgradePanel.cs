using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        upgradeButton_01.color = Color.white;
        upgradeButton_02.color = defaultButtonColor;
        upgradeButton_03.color = defaultButtonColor;
    }

    public void OnPressUpgradeButton02()
    {
        infoPanel.gameObject.SetActive(true);
        upgradeButton_01.color = defaultButtonColor;
        upgradeButton_02.color = Color.white;
        upgradeButton_03.color = defaultButtonColor;
    }

    public void OnPressUpgradeButton03()
    {
        infoPanel.gameObject.SetActive(true);
        upgradeButton_01.color = defaultButtonColor;
        upgradeButton_02.color = defaultButtonColor;
        upgradeButton_03.color = Color.white;

    }
}
