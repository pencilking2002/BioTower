using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using BioTower.Structures;
using BioTower.SaveData;
using TMPro;

namespace BioTower.UI
{
    public class GameplayUI : MonoBehaviour
    {
        public CanvasGroup panel;
        [SerializeField] private Button AbaTowerButton;
        [SerializeField] private Button Pp2cTowerButton;
        [SerializeField] private Button chloroplastTowerButton;
        [SerializeField] private Button mitoTowerButton;
        [SerializeField] private Button currSelectedBtn;
        public GameObject currencyContainer;
        [SerializeField] private TextMeshProUGUI playerCurrencyText;
        public Dictionary<StructureType, Button> towerButtonMap = new Dictionary<StructureType, Button>();
        private Vector3 initPos;

        private void Awake()
        {
            towerButtonMap.Add(StructureType.ABA_TOWER, AbaTowerButton);
            towerButtonMap.Add(StructureType.PPC2_TOWER, Pp2cTowerButton);
            towerButtonMap.Add(StructureType.CHLOROPLAST, chloroplastTowerButton);
            towerButtonMap.Add(StructureType.MITOCHONDRIA, mitoTowerButton);
            initPos = panel.transform.position;
        }

        private void Start()
        {
            SetTowerPrice(StructureType.ABA_TOWER);
            SetTowerPrice(StructureType.PPC2_TOWER);
            SetTowerPrice(StructureType.CHLOROPLAST);
            SetTowerPrice(StructureType.MITOCHONDRIA);
        }

        private void OnLevelStart(LevelType levelType)
        {
            bool ppc2TowerUnlocked = Util.upgradeSettings.ppc2TowerUnlocked;
            Pp2cTowerButton.gameObject.SetActive(ppc2TowerUnlocked);

            bool chloroTowerUnlocked = Util.upgradeSettings.chloroTowerUnlocked;
            chloroplastTowerButton.gameObject.SetActive(chloroTowerUnlocked);

            bool mitoTowerUnlocked = Util.upgradeSettings.mitoTowerUnlocked;
            mitoTowerButton.gameObject.SetActive(mitoTowerUnlocked);

            if (LevelInfo.current.IsFirstLevel())
            {
                PositionPanelOffScreen();
            }
            else
            {
                PositionPanelOffScreen();
                SlideInPanel(2.0f);
            }
        }

        private void PositionPanelOffScreen()
        {
            var startPos = initPos;
            startPos.y -= 120;
            panel.transform.position = startPos;
        }

        private void SlideInPanel(float delay)
        {
            LeanTween.delayedCall(gameObject, delay, () =>
            {
                LeanTween.move(panel.gameObject, initPos, 0.5f).setEaseOutQuint();
            });
        }

        public StructureType GetSelectedButtonType()
        {
            foreach (KeyValuePair<StructureType, Button> btn in towerButtonMap)
            {
                if (btn.Value == currSelectedBtn)
                    return btn.Key;
            }
            return StructureType.NONE;
        }

        ///<Summary>
        /// Sets the tower price on the button
        ///</Summary>
        private void SetTowerPrice(StructureType structureType)
        {
            var text = towerButtonMap[structureType].transform.Find("Panel").Find("PriceText").GetComponent<Text>();
            int cost = Util.gameSettings.upgradeSettings.GetTowerCost(structureType);
            text.text = cost.ToString();
        }

        private void Update()
        {
            playerCurrencyText.text = GameManager.Instance.econManager.playerCurrency.ToString();
        }


        // BUTTON METHODS -------------------------------------------------------------------------------


        public void OnPressAbaTowerButton()
        {
            //Debug.Log("Try to press ABA tower btn");

            if (!Util.towerManager.HasAvailableSockets())
            {
                Util.HandleInvalidButtonPress(AbaTowerButton);
                //Debug.Log("No available sockets");
                return;
            }

            if (!InputController.canPressButtons)
            {
                Util.HandleInvalidButtonPress(AbaTowerButton);
                //Debug.Log("can't press buttons");
                return;
            }

            bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.ABA_TOWER];
            if (!canBuildTower)
            {
                //Debug.Log("can't press buttons");
                return;
            }

            if (GameManager.Instance.econManager.CanBuyTower(StructureType.ABA_TOWER))
            {
                HandleButtonPress(AbaTowerButton, StructureType.ABA_TOWER);
                if (LevelInfo.current.IsFirstLevel())
                {
                    Util.HideGlowUI(AbaTowerButton.transform);
                }
            }
            else
            {
                Util.HandleInvalidButtonPress(AbaTowerButton);
                GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
                // Debug.Log("can't buy tower");
            }


        }

        public void OnPressPPC2TowerButton()
        {
            if (!Util.towerManager.HasAvailableSockets())
            {
                Util.HandleInvalidButtonPress(AbaTowerButton);
                return;
            }

            bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.PPC2_TOWER];
            if (!canBuildTower)
                return;

            if (GameManager.Instance.econManager.CanBuyTower(StructureType.PPC2_TOWER))
            {
                HandleButtonPress(Pp2cTowerButton, StructureType.PPC2_TOWER);
            }
            else
            {
                Util.HandleInvalidButtonPress(Pp2cTowerButton);
                GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
            }
        }

        public void OnPressChloroplastButton()
        {
            if (!Util.towerManager.HasAvailableSockets())
            {
                Util.HandleInvalidButtonPress(AbaTowerButton);
                return;
            }

            bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.CHLOROPLAST];
            if (!canBuildTower)
                return;

            if (GameManager.Instance.econManager.CanBuyTower(StructureType.CHLOROPLAST))
            {
                HandleButtonPress(chloroplastTowerButton, StructureType.CHLOROPLAST);
            }
            else
            {
                Util.HandleInvalidButtonPress(chloroplastTowerButton);
                GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
            }
        }

        public void OnPressMitoButton()
        {
            if (!Util.towerManager.HasAvailableSockets())
            {
                Util.HandleInvalidButtonPress(AbaTowerButton);
                return;
            }

            bool canBuildTower = CooldownManager.structureCooldownMap[StructureType.MITOCHONDRIA];
            if (!canBuildTower)
                return;

            if (GameManager.Instance.econManager.CanBuyTower(StructureType.MITOCHONDRIA))
            {
                HandleButtonPress(mitoTowerButton, StructureType.MITOCHONDRIA);
            }
            else
            {
                Util.HandleInvalidButtonPress(mitoTowerButton);
                GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
            }
        }

        private void HandleButtonPress(Button button, StructureType structureType)
        {
            AnimateButton(button);
            currSelectedBtn = button;
            EventManager.UI.onPressTowerButton?.Invoke(structureType);
            EventManager.UI.onTapButton?.Invoke(true);
        }

        private void AnimateButton(Button button)
        {
            if (button == currSelectedBtn)
                return;

            var initPos = button.transform.localPosition;
            if (currSelectedBtn != null)
                LeanTween.moveLocalY(currSelectedBtn.gameObject, initPos.y, 0.1f);

            LeanTween.moveLocalY(button.gameObject, initPos.y + 20, 0.1f);

        }

        private void HandleButtonColor(Button button)
        {
            var image = button.transform.Find("Panel").GetComponent<Image>();
            var oldColor = image.color;
            image.color = Color.grey;
            LeanTween.delayedCall(gameObject, GameManager.Instance.cooldownManager.structureSpawnCooldown, () =>
            {
                image.color = oldColor;
            });
        }

        private void PingPongScaleCurrencyUI(float targetScale)
        {
            LeanTween.cancel(playerCurrencyText.gameObject);
            playerCurrencyText.transform.localScale = Vector3.one;
            var oldScale = playerCurrencyText.transform.localScale;
            LeanTween.scale(playerCurrencyText.gameObject, oldScale * targetScale, 0.1f).setLoopPingPong(1);
        }

        private void OnSpendCurrency(int numSpent, int currTotal)
        {
            PingPongScaleCurrencyUI(1.2f);
        }

        private void OnGainCurrency(int numGained, int currTotal)
        {
            //Debug.Log($"Currency gained");
            PingPongScaleCurrencyUI(1.2f);
        }

        private void OnStructureCooldownStarted(StructureType structureType, float cooldown)
        {
            if (!towerButtonMap.ContainsKey(structureType))
                return;

            DeselectCurrentButton();

            var button = towerButtonMap[structureType];

            button.interactable = false;
            var cooldownImage = button.transform.Find("Cooldown").GetComponent<Image>();
            LeanTween.value(gameObject, 1, 0, cooldown).setOnUpdate((float val) =>
            {
                cooldownImage.fillAmount = val;
            })
            .setOnComplete(() =>
            {
                // Don't make button interactable if its the first level and there's currently tutorials playing
                if (LevelInfo.current.IsFirstLevel() && Util.tutCanvas.hasTutorials)
                    button.interactable = false;
                else
                    button.interactable = true;
            });

            HandleButtonColor(button);
        }


        private void DeselectCurrentButton()
        {
            // Deselect current button
            if (currSelectedBtn != null)
            {
                LeanTween.moveLocalY(currSelectedBtn.gameObject, currSelectedBtn.transform.localPosition.y - 20, 0.1f);
                currSelectedBtn = null;
            }
        }

        private void OnSetNonePlacementState()
        {
            DeselectCurrentButton();
        }

        private void OnTutorialStart(TutorialData data)
        {
            if (LevelInfo.current.IsFirstLevel())
            {
                if (data.IsTapAbaButtonRequiredAction())
                    SlideInPanel(0);
            }
        }

        private void OnHighlightItem(HighlightedItem item)
        {
            if (item == HighlightedItem.ABA_TOWER_BTN)
            {
                LeanTween.delayedCall(0.5f, () =>
                {
                    var worldPos = Camera.main.ScreenToWorldPoint(AbaTowerButton.transform.position);
                    Util.poolManager.SpawnItemHighlight(worldPos, new Vector2(0, 150));
                });
            }
            else if (item == HighlightedItem.ENERGY)
            {
                //var worldPos = Camera.main.ScreenToWorldPoint(currencyContainer.transform.position);
                //Util.poolManager.SpawnItemHighlight(worldPos, new Vector2(0,150));
                currencyContainer.transform.Find("Glow").GetComponent<Image>().enabled = true;
            }

            if (item != HighlightedItem.ENERGY)
                currencyContainer.transform.Find("Glow").GetComponent<Image>().enabled = false;
        }

        private void OnTutorialEnd(TutorialData data)
        {
            if (LevelInfo.current.IsFirstLevel())
            {
                Util.DisplayGlowUI(AbaTowerButton.transform);
                AbaTowerButton.interactable = true;
                //Util.tutCanvas.hasTutorials && Util.tutCanvas.currTutorial.IsSpawnAbaUnitRequiredAction())

            }
        }

        private void OnSkipTutorials()
        {
            if (!Mathf.Approximately(panel.transform.position.y, initPos.y))
                SlideInPanel(1.0f);
        }

        private async void OnEnable()
        {
            EventManager.Tutorials.onTutorialStart += OnTutorialStart;
            EventManager.Tutorials.onHighlightItem += OnHighlightItem;
            EventManager.Tutorials.onSkipTutorials += OnSkipTutorials;
            EventManager.Tutorials.onTutorialEnd += OnTutorialEnd;
            EventManager.Game.onLevelStart += OnLevelStart;
            EventManager.Game.onSpendCurrency += OnSpendCurrency;
            EventManager.Game.onGainCurrency += OnGainCurrency;
            EventManager.Structures.onStructureCooldownStarted += OnStructureCooldownStarted;
            EventManager.Structures.onSetNonePlacementState += OnSetNonePlacementState;
        }

        private void OnDisable()
        {
            EventManager.Tutorials.onTutorialStart -= OnTutorialStart;
            EventManager.Tutorials.onHighlightItem -= OnHighlightItem;
            EventManager.Tutorials.onSkipTutorials -= OnSkipTutorials;
            EventManager.Tutorials.onTutorialEnd -= OnTutorialEnd;
            EventManager.Game.onLevelStart -= OnLevelStart;
            EventManager.Game.onSpendCurrency -= OnSpendCurrency;
            EventManager.Game.onGainCurrency -= OnGainCurrency;
            EventManager.Structures.onStructureCooldownStarted -= OnStructureCooldownStarted;
            EventManager.Structures.onSetNonePlacementState -= OnSetNonePlacementState;
        }
    }
}
