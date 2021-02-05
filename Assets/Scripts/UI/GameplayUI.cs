using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using BioTower.Structures;

namespace BioTower.UI
{
public class GameplayUI : MonoBehaviour
{
    public static Action<StructureType> onTowerButton;
    [SerializeField] private CanvasGroup gameUIPanel;
    [SerializeField] private Button spawnAbaTowerButton;

    public void OnPressAbaTowerButton()
    {
        onTowerButton?.Invoke(StructureType.ABA_TOWER);
    }
}
}
