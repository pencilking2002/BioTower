using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using BioTower.Structures;
using System;

namespace BioTower.UI
{
    public class TowerPopUpCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform panel;
        [SerializeField] private RectTransform destroyTowerBtn;
        [SerializeField] private RectTransform spawnUnitBtn;
        private Vector3 initScale;
        [HideInInspector] public bool isDisplayed;

        private void Awake()
        {
            initScale = panel.localScale;
        }

        private void Start()
        {
            destroyTowerBtn.gameObject.SetActive(!LevelInfo.current.IsFirstLevel());
        }

        public void OnPressDestroyTowerBtn()
        {
            Hide(0.25f, () =>
            {
                EventManager.UI.onPressTowerDestroyedBtn?.Invoke(Util.tapManager.selectedStructure);
            });
        }

        public void OnPressSpawnUnitButton()
        {
            var selectedStructure = GameManager.Instance.tapManager.selectedStructure;

            UnitType unitType = UnitType.ABA;
            if (selectedStructure.IsAbaTower())
            {
                if (GameManager.Instance.econManager.CanBuyUnit(unitType))
                {
                    var abaTower = (ABATower)selectedStructure;
                    if (abaTower.IsBelowSpawnLimit())
                    {
                        GameManager.Instance.econManager.BuyUnit(unitType);
                        GameManager.Instance.tapManager.selectedStructure.SpawnUnits(1);
                    }
                }
                else
                {
                    var currencyContainer = GameManager.Instance.bootController.gameplayUI.currencyContainer;
                    GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
                }
            }
            else if (selectedStructure.IsPPC2Tower())
            {
                PPC2Tower ppc2Tower = (PPC2Tower)selectedStructure;

                if (ppc2Tower.IsBelowSpawnLimit())
                {
                    unitType = UnitType.SNRK2;
                    if (GameManager.Instance.econManager.CanBuyUnit(unitType))
                    {
                        GameManager.Instance.econManager.BuyUnit(unitType);
                        GameManager.Instance.tapManager.selectedStructure.SpawnUnits(1);
                    }
                    else
                    {
                        var currencyContainer = GameManager.Instance.bootController.gameplayUI.currencyContainer;
                        GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
                    }
                }
            }
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
            panel.LeanScale(initScale, duration).setEaseOutBack();
            isDisplayed = true;
        }

        public void Hide(float duration, Action onComplete = null)
        {
            if (!isDisplayed)
                return;

            if (Mathf.Approximately(duration, 0))
            {
                panel.gameObject.SetActive(false);
                return;
            }

            panel.LeanScale(Vector3.zero, duration)
            .setEaseInBack()
            .setOnComplete(() =>
            {
                panel.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
            isDisplayed = false;
        }
    }
}
