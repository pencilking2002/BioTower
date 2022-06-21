using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BioTower
{
    public class PopUpTutorialController : MonoBehaviour
    {
        public bool hasPopUp;
        [ShowIf("hasPopUp")][SerializeField] private RectTransform container;
        [ShowIf("hasPopUp")][SerializeField] private RectTransform panel;
        [ShowIf("hasPopUp")][SerializeField] private CanvasGroup darkBG;

        [Header("Settings")]
        [ShowIf("hasPopUp")][SerializeField] private float slideInDuration = 0.25f;
        [ShowIf("hasPopUp")][SerializeField] private float slideOutDuration = 0.25f;

        [ReadOnly] public bool isDisplayed;
        private Vector3 onScreenPos;
        private Vector3 offscreenPos;

        private void Awake()
        {
            if (!hasPopUp)
            {
                this.enabled = false;
                return;
            }
            container.gameObject.SetActive(false);
            onScreenPos = panel.transform.position;
            offscreenPos = onScreenPos + new Vector3(0, 900, 0);

            DisplayPanel(1);
        }

        public void DisplayPanel(float delay = 0)
        {
            panel.transform.position = offscreenPos;
            darkBG.alpha = 0;
            LeanTween.delayedCall(gameObject, delay, () =>
            {
                Time.timeScale = 0;
                isDisplayed = true;
                container.gameObject.SetActive(true);
                LeanTween.move(panel.gameObject, onScreenPos, slideInDuration).setEaseOutQuad().setIgnoreTimeScale(true);
                LeanTween.alphaCanvas(darkBG, 1, slideInDuration).setIgnoreTimeScale(true);
            }).setIgnoreTimeScale(true);
        }

        public void HidePanel()
        {
            isDisplayed = false;
            darkBG.alpha = 1;
            LeanTween.move(panel.gameObject, offscreenPos, slideOutDuration).setOnComplete(() =>
            {
                Time.timeScale = 1;
                container.gameObject.SetActive(false);
            }).setIgnoreTimeScale(true);

            LeanTween.alphaCanvas(darkBG, 0, slideOutDuration).setIgnoreTimeScale(true);
        }
    }
}
