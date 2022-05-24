using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BioTower
{
    public class PopUpTutorialController : MonoBehaviour
    {
        [SerializeField] private bool hasPopUp;
        [ShowIf("hasPopUp")][SerializeField] private RectTransform panel;
    }
}
