using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.UI
{
    public class TowerPopUpCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform panel;
        private Vector3 initScale;
        private bool isDisplayed;

        private void Awake()
        {
            initScale = panel.localScale;
        }

        public void Display(float duration = 0)
        {
            if (isDisplayed)
                return;

            if (Mathf.Approximately(duration, 0))
            {
                panel.gameObject.SetActive(true);
                return;
            }

            panel.gameObject.SetActive(true);
            panel.localScale = Vector3.zero;
            panel.LeanScale(initScale, duration);
            isDisplayed = true;
        }

        public void Hide(float duration = 0)
        {
            if (!isDisplayed)
                return;

            if (Mathf.Approximately(duration, 0))
            {
                panel.gameObject.SetActive(false);
                return;
            }

            panel.LeanScale(Vector3.zero, duration).setOnComplete(() =>
            {
                panel.gameObject.SetActive(false);
            });
            isDisplayed = false;
        }
    }
}
