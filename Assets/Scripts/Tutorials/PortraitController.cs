using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitController : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private Sprite[] portraits;

    public void SetPortrait(int i)
    {
        portrait.sprite = portraits[i];
    }
}
