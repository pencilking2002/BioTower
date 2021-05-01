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
    [SerializeField] private CanvasGroup gameUIPanel;
    [SerializeField] private Button spawnAbaTowerButton;

    public void OnPressAbaTowerButton()
    {
        EventManager.UI.onPressTowerButton?.Invoke(StructureType.ABA_TOWER);
    }
}
}
