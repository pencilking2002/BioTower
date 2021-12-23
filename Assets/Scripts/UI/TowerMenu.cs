using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using UnityEngine.UI;
using TMPro;
using BioTower.Units;

namespace BioTower
{
public class TowerMenu : MonoBehaviour
{
    public RectTransform towerPanel;
   
    [SerializeField] private Slider towerHealthbar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Button healTowerButton;
    [SerializeField] private Button healTowerFullWidthButton;
    [SerializeField] private Button spawnUnitButton;
    [SerializeField] private Button spawnUnitFullWidth;
    [SerializeField] private Button spawnLightParticleButton;
    private Image spawnLightDropCooldownImage;
    [SerializeField] private TextMeshProUGUI currTowerText;

    
    [Header("Tower Icons")]
    [SerializeField] private Image towerIcon;
    [SerializeField] private Sprite playerTower;
    [SerializeField] private Sprite abaTower;
    [SerializeField] private Sprite pp2cTower;
    [SerializeField] private Sprite chloroplastTower;
    [SerializeField] private Sprite mitoTower;
    private Dictionary<StructureType, Sprite> iconMap = new Dictionary<StructureType, Sprite>();

    private void Awake()
    {
        spawnLightDropCooldownImage = spawnLightParticleButton.transform.Find("Cooldown").GetComponent<Image>();
        towerPanel.gameObject.SetActive(false);
        iconMap.Add(StructureType.ABA_TOWER, abaTower);
        iconMap.Add(StructureType.PPC2_TOWER, pp2cTower);
        iconMap.Add(StructureType.CHLOROPLAST, chloroplastTower);
        iconMap.Add(StructureType.DNA_BASE, playerTower);
        iconMap.Add(StructureType.MITOCHONDRIA, mitoTower);
        iconMap.Add(StructureType.MINI_CHLOROPLAST_TOWER, mitoTower);
    }

    private void Start()
    {
        SetPrice(healTowerButton);
        SetPrice(healTowerFullWidthButton);
        SetPrice(spawnUnitButton);
        SetPrice(spawnUnitFullWidth);
        SetPrice(spawnLightParticleButton);
    }

    private void Update()
    {
        if (!Util.gameStates.IsGameState())
            return;

        HandleMitoTowerCooldownDisplay();
    }

    private void HandleMitoTowerCooldownDisplay()
    {
        if (Util.tapManager.hasSelectedStructure && Util.tapManager.selectedStructure.IsMitoTower())
        {
            var tower = (MitoTower) Util.tapManager.selectedStructure;

            if (tower.isCoolingDown)
            {
                var totalTime = tower.spawnLightFragCooldown;
                var timeLeft = Time.time - tower.cooldownStartTime;
                var percentage = 1-(timeLeft/totalTime);
                spawnLightDropCooldownImage.fillAmount = percentage;
            }
            else
            {
                spawnLightDropCooldownImage.fillAmount = 0;
            }
        }
    }

    private void SetPrice(Button button)
    {
        Text text = button.transform.Find("PriceText").GetComponent<Text>();
        if (button == healTowerButton || button == healTowerFullWidthButton)
            text.text = Util.gameSettings.upgradeSettings.healTowerCost.ToString();
        else if (button == spawnUnitButton || button == spawnUnitFullWidth)
            text.text = Util.gameSettings.upgradeSettings.abaUnitCost.ToString();
         else if (button == spawnLightParticleButton)
            text.text = Util.gameSettings.spawnLightDropCost.ToString();
    }

    public void OnPressSpawnUnitButton()
    {

        if (TutorialCanvas.tutorialInProgress)
        {
            if (Util.tutCanvas.tutState != TutState.WAITING_BUTTON_TAP)
            {
                Util.HandleInvalidButtonPress(spawnUnitFullWidth, Util.ButtonColorMode.DEFAULT);
                return;
            }
        }
        
        var selectedStructure = GameManager.Instance.tapManager.selectedStructure;

        UnitType unitType = UnitType.ABA;
        if (selectedStructure.IsAbaTower())
        {
            unitType = UnitType.ABA;
            if (GameManager.Instance.econManager.CanBuyUnit(unitType))
            {
                var abaTower = (ABATower) selectedStructure;
                if (abaTower.IsBelowSpawnLimit())
                {
                    GameManager.Instance.econManager.BuyUnit(unitType);
                    GameManager.Instance.tapManager.selectedStructure.SpawnUnits(1);

                    // Inform user that you can't spawn any more aba units on this tower
                    if (!abaTower.IsBelowSpawnLimit())
                    {
                        SetButtonMaxText(spawnUnitButton);
                    }
                    else
                    {
                        if (LevelInfo.current.IsFirstLevel())
                            Util.HideGlowUI(spawnUnitFullWidth.transform);
                    }

                    EventManager.UI.onTapSpawnUnitButton?.Invoke(unitType);
                    EventManager.UI.onTapButton?.Invoke(true);
                }
            }
            else
            {
                Util.HandleInvalidButtonPress(spawnUnitButton, Util.ButtonColorMode.DEFAULT);
                var currencyContainer = GameManager.Instance.bootController.gameplayUI.currencyContainer;
                GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
            }
        }
        else if (selectedStructure.IsPPC2Tower())
        {
            PPC2Tower ppc2Tower = (PPC2Tower) selectedStructure;

            if (ppc2Tower.IsBelowSpawnLimit())
            { 
                unitType = UnitType.SNRK2;
                if (GameManager.Instance.econManager.CanBuyUnit(unitType))
                {
                    GameManager.Instance.econManager.BuyUnit(unitType);
                    GameManager.Instance.tapManager.selectedStructure.SpawnUnits(1);
                    EventManager.UI.onTapSpawnUnitButton?.Invoke(unitType);
                    EventManager.UI.onTapButton?.Invoke(true);
                }
                else
                {
                    Util.HandleInvalidButtonPress(spawnUnitButton, Util.ButtonColorMode.DEFAULT);
                    var currencyContainer = GameManager.Instance.bootController.gameplayUI.currencyContainer;
                    GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
                }
            }
        }
    }

    public void OnPressHealTowerButton()
    {
        var tower = Util.tapManager.selectedStructure;

        if (GameManager.Instance.econManager.CanBuyTowerHeal() && !tower.IsMaxHealth())
        {
            var healAmount = Util.gameSettings.healTowerAmount;
            GameManager.Instance.tapManager.selectedStructure.GainHealth(healAmount);
            GameManager.Instance.econManager.BuyTowerHeal();
            var selectedTower = GameManager.Instance.tapManager.selectedStructure;
            EventManager.UI.onTapButton?.Invoke(true);
        }
        else
        {
            Util.HandleInvalidButtonPress(healTowerButton, Util.ButtonColorMode.DEFAULT);
            var currencyContainer = GameManager.Instance.bootController.gameplayUI.currencyContainer;
            GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
        }
    }

    public void OnPressLightDropButton()
    {
        if (Util.econManager.CanBuyLightFragment())
        {
            if (spawnLightParticleButton.IsInteractable())
            {
                var selectedTower = GameManager.Instance.tapManager.selectedStructure;
                var mitoTower = (MitoTower) selectedTower;
                mitoTower.ShootFragment();
                Util.econManager.BuyLightFragment();
                EventManager.UI.onTapLightDropButton?.Invoke(mitoTower);
                spawnLightParticleButton.interactable = false;
            }
        }
        else
        {
            Util.HandleInvalidButtonPress(spawnLightParticleButton, Util.ButtonColorMode.DEFAULT);
            var currencyContainer = GameManager.Instance.bootController.gameplayUI.currencyContainer;
            GameManager.Instance.objectShake.ShakeHorizontal(currencyContainer, 0.15f, 5.0f);
        }
    }

    public void OnStructureSelected(Structure structure)
    {
        if (structure.IsBarrier() || structure.IsMiniChloroTower())
            return;
            
        //Debug.Log($"Tap {tower.structureType}");
        bool displaySpawnUnitButton = structure.IsAbaTower() || 
                                    (structure.IsPPC2Tower() && Util.upgradeSettings.snrk2UnitUnlocked);
                                    
        bool displayLightDropButton = structure.structureType == StructureType.MITOCHONDRIA;

        spawnUnitButton.gameObject.SetActive(displaySpawnUnitButton);
        var spawnUnitText = spawnUnitButton.transform.Find("Text").GetComponent<Text>();

        if (structure.IsAbaTower())
        {
            //spawnUnitText.text = "ABA\nUnit";
            var abaTower = (ABATower) structure;
            if (abaTower.IsBelowSpawnLimit())
                SetButtonTextDefault(spawnUnitButton, "ABA\nUnit");
            else
                SetButtonMaxText(spawnUnitButton);
        }
        else if (structure.IsPPC2Tower())
        {
            var ppc2Tower = (PPC2Tower) structure;
            if (ppc2Tower.IsBelowSpawnLimit())
                SetButtonTextDefault(spawnUnitButton, "SNRK2\nUnit");
            else
                SetButtonMaxText(spawnUnitButton);
        }
        else if (structure.IsMitoTower())
        {
            var tower = (MitoTower) structure;
            spawnLightParticleButton.interactable = !tower.isCoolingDown;
        }

        spawnLightParticleButton.gameObject.SetActive(displayLightDropButton);
        currTowerText.text = structure.structureType.ToString().Replace('_', ' ');
        towerPanel.gameObject.SetActive(true);
        healTowerButton.gameObject.SetActive((displaySpawnUnitButton || displayLightDropButton) && !LevelInfo.current.IsFirstLevel());
        healTowerFullWidthButton.gameObject.SetActive((!displaySpawnUnitButton && !displayLightDropButton) && !LevelInfo.current.IsFirstLevel());
        spawnUnitFullWidth.gameObject.SetActive(LevelInfo.current.IsFirstLevel());
        UpdateTowerHealthBar(structure);
        towerIcon.sprite = iconMap[structure.structureType];
    }
    
    private void SetButtonMaxText(Button btn)
    {
        var text = btn.transform.Find("Text").GetComponent<Text>();
        text.text = "MAX";
        text.color = Color.red;
        btn.transform.Find("PriceText").gameObject.SetActive(false);
    }

    private void SetButtonTextDefault(Button btn, string text)
    {
        var textComponent = btn.transform.Find("Text").GetComponent<Text>();
        textComponent.text = text;
        textComponent.color = Color.black;
        btn.transform.Find("PriceText").gameObject.SetActive(true);
    }

    private void UpdateTowerHealthBar(Structure structure, float duration=0)
    {
        if (structure == GameManager.Instance.tapManager.selectedStructure && structure.hasHealth)
        {
            towerHealthbar.maxValue = structure.GetMaxHealth();

            LeanTween.value(gameObject, towerHealthbar.value, structure.GetCurrHealth(), duration)
            .setOnUpdate((float val) => {
                towerHealthbar.value = val;
            });
            healthText.text = $"{structure.GetCurrHealth()}/{structure.GetMaxHealth()}"; 
        }
    }

    private void OnStructureCreated(Structure structure)
    {
        if (structure.structureType == StructureType.DNA_BASE)
            return;
            
        OnStructureSelected(structure);
    }

    private void OnStructureGainHealth(Structure structure)
    {
        UpdateTowerHealthBar(structure);
    }

    private void OnStructureLoseHealth(Structure structure)
    {
        UpdateTowerHealthBar(structure, 0.25f);
    }

    private void OnUnitDestroyed(Unit unit)
    {
        if (!spawnUnitButton.gameObject.activeInHierarchy)
            return;

        var selectedStructure = GameManager.Instance.tapManager.selectedStructure;
        if (unit.tower == selectedStructure)
        {
            if (selectedStructure.IsAbaTower())
            {   
                SetButtonTextDefault(spawnUnitButton, "ABA\nUnit");
            }
            else if (selectedStructure.IsPPC2Tower())
            {   
                SetButtonTextDefault(spawnUnitButton, "SNRK2\nUnit");
            }     
        }
    }
  
    private void OnHighlightItem(HighlightedItem item)
    {
        if (LevelInfo.current.IsFirstLevel() && item == HighlightedItem.ABA_UNIT_BTN)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(spawnUnitFullWidth.transform.position);
            Util.poolManager.SpawnItemHighlight(worldPos, new Vector2(0,130)); 
            Util.DisplayGlowUI(spawnUnitFullWidth.transform);
        }
        else
        {

        }
    }

    /// <summary>
    /// If mito tower is selected, update the spawn light drops button
    /// </summary>
    /// <param name="structure"></param>
    private void OnSpawnLightDropCooldownComplete(Structure structure)
    {
        var selectedStructure = GameManager.Instance.tapManager.selectedStructure;
        if (selectedStructure == structure && structure.IsMitoTower())
        {
            //var tower = (MitoTower) selectedStructure;
            spawnLightParticleButton.interactable = true;
        }
    }

    private void OnTutorialStart(TutorialData data)
    {
        if (LevelInfo.current.IsFirstLevel())
        {
            // If its the tutorial right after the unit spawn tut
            if (Util.tutCanvas.currTutorialIndex >= 6)
            {
                spawnUnitFullWidth.interactable = false;
                spawnUnitButton.gameObject.SetActive(false);
                Debug.Log("disable unit spawn button");
            }
        }
    }

    private void OnTutorialEnd(TutorialData data)
    {
        if (LevelInfo.current.IsFirstLevel())
        {
            spawnUnitFullWidth.interactable = true;
            spawnUnitButton.gameObject.SetActive(true);
            Debug.Log("Enable unit spawn button");
        }
    }

    private void OnEnable()
    {
        EventManager.Tutorials.onTutorialStart += OnTutorialStart;
        EventManager.Tutorials.onHighlightItem += OnHighlightItem;
        EventManager.Tutorials.onTutorialEnd += OnTutorialEnd;
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        EventManager.Structures.onStructureGainHealth += OnStructureGainHealth;
        EventManager.Structures.onStructureLoseHealth += OnStructureLoseHealth;
        EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
        EventManager.UI.onSpawnLightDropCooldownComplete += OnSpawnLightDropCooldownComplete;
    }

    private void OnDisable()
    {
        EventManager.Tutorials.onTutorialStart -= OnTutorialStart;
        EventManager.Tutorials.onHighlightItem -= OnHighlightItem;
        EventManager.Tutorials.onTutorialEnd -= OnTutorialEnd;
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        EventManager.Structures.onStructureGainHealth -= OnStructureGainHealth;
        EventManager.Structures.onStructureLoseHealth -= OnStructureLoseHealth;
        EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;        
        EventManager.UI.onSpawnLightDropCooldownComplete -= OnSpawnLightDropCooldownComplete;
    }
}
}