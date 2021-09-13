using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace BioTower
{
[TypeInfoBox("Defines text and sprite data associated with each upgrade so it can be properly displayed in the Upgrade Panel")]

[CreateAssetMenu(fileName="UpgradeTextData", menuName="UpgradeTextData")]
public class UpgradeTextData : ScriptableObject
{
    [TabGroup("ABA")] public UpgradeData[] abaTowerDataArr;
    [TabGroup("PP2C")] public UpgradeData[] ppc2TowerDataArr;
    [TabGroup("Mito")] public UpgradeData[] mitoTowerDataArr;
    [TabGroup("Chloroplast")] public UpgradeData[] chloroTowerDataArr;
    [TabGroup("Player")] public UpgradeData[] playerTowerDataArr;


    public UpgradeData GetUpgradeTextData(UpgradeType upgradeType)
    {
        foreach(UpgradeData data in abaTowerDataArr)
        {
            if (data.upgradeType == upgradeType) return data;
        }
        foreach(UpgradeData data in ppc2TowerDataArr)
        {
            if (data.upgradeType == upgradeType) return data;
        }
        foreach(UpgradeData data in mitoTowerDataArr)
        {
            if (data.upgradeType == upgradeType) return data;
        }
        foreach(UpgradeData data in chloroTowerDataArr)
        {
            if (data.upgradeType == upgradeType) return data;
        }
        foreach(UpgradeData data in playerTowerDataArr)
        {
            if (data.upgradeType == upgradeType) return data;
        }
        return null;
    }

}

[Serializable]
public class UpgradeData
{
    public UpgradeType upgradeType;
    public string buttonText;
    [Multiline] public string descrptionText;
    public Sprite sprite;
}
}