using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BioTower
{
public class PortraitController : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private Sprite[] portraits;

    public void SetPortrait(int i)
    {
        if (i < portraits.Length)
            portrait.sprite = portraits[i];
        else
            Debug.LogError($"The portrait index {i} is too high for {Util.tutCanvas.currTutorial.name}");
    }
}
}