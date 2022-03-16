using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BioTower.UI
{
    public class Bar : MonoBehaviour
    {
        private Image image;
        [HideInInspector] public RectTransform rt;
        [HideInInspector] public RectTransform innerRT;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            image = rt.GetChild(0).GetComponent<Image>();
            innerRT = image.GetComponent<RectTransform>();
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}
