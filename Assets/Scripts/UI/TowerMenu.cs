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
    [SerializeField] private Button spawnLightParticleButton;
    //[SerializeField] private float scaleAnimDuration = 0.1f;
    [SerializeField] private Text currTowerText;

    
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
        towerPanel.gameObject.SetActive(false);
        iconMap.Add(StructureType.ABA_TOWER, abaTower);
        iconMap.Add(StructureType.PPC2_TOWER, pp2cTower);
        iconMap.Add(StructureType.CHLOROPLAST, chloroplastTower);
        iconMap.Add(StructureType.DNA_BASE, playerTower);
        iconMap.Add(StructureType.MITOCHONDRIA, mitoTower);
    }

    private void Start()
    {
        SetPrice(healTowerButton);
        SetPrice(healTowerFullWidthButton);
        SetPrice(spawnUnitButton);
        SetPrice(spawnLightParticleButton);
    }

    private void SetPrice(Button button)
    {
        Text text = button.transform.Find("PriceText").GetComponent<Text>();
        if (button == healTowerButton || button == healTowerFullWidthButton)
            text.text = Util.gameSettings.upgradeSettings.healTowerCost.ToString();
        else if (button == spawnUnitButton)
            text.text = Util.gameSettings.upgradeSettings.abaUnitCost.ToString();
         else if (button == spawnLightParticleButton)
            text.text = Util.gameSettings.spawnLightDropCost.ToString();
    }

    public void OnPressSpawnUnitButton()
    {
        var selectedStructure = GameManager.Instance.tapManager.selectedStructure;

        UnitType unitType = UnitType.ABA;
        if (selectedStructure.structureType == StructureType.ABA_TOWER)
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
        else if (selectedStructure.structureType == StructureType.PPC2_TOWER)
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
        if (GameManager.Instance.econManager.CanBuyTowerHeal())
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
            var selectedTower = GameManager.Instance.tapManager.selectedStructure;
            var mitoTower = (MitoTower) selectedTower;
            mitoTower.ShootFragment();
            Util.econManager.BuyLightFragment();
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
        if (structure.structureType == StructureType.ROAD_BARRIER)
            return;
            
        //Debug.Log($"Tap {tower.structureType}");
        bool displaySpawnUnitButton = structure.structureType == StructureType.ABA_TOWER || 
                                    (structure.structureType == StructureType.PPC2_TOWER && Util.upgradeSettings.snrk2UnitUnlocked);
                                    
        bool displayLightDropButton = structure.structureType == StructureType.MITOCHONDRIA;

        spawnUnitButton.gameObject.SetActive(displaySpawnUnitButton);
        var spawnUnitText = spawnUnitButton.transform.Find("Text").GetComponent<Text>();

        if (structure.structureType == StructureType.ABA_TOWER)
        {
            //spawnUnitText.text = "ABA\nUnit";
            var abaTower = (ABATower) structure;
            if (abaTower.IsBelowSpawnLimit())
                SetButtonTextDefault(spawnUnitButton, "ABA\nUnit");
            else
                SetButtonMaxText(spawnUnitButton);
        }
        else if (structure.structureType == StructureType.PPC2_TOWER)
        {
            var ppc2Tower = (PPC2Tower) structure;
            if (ppc2Tower.IsBelowSpawnLimit())
             SetButtonTextDefault(spawnUnitButton, "SNRK2\nUnit");
            else
                SetButtonMaxText(spawnUnitButton);

            //spawnUnitText.text = "SNRK2\nUnit";

            // TODO: Create spawn limit logic for ppc2
        }

        spawnLightParticleButton.gameObject.SetActive(displayLightDropButton);
        currTowerText.text = structure.structureType.ToString().Replace('_', ' ');
        towerPanel.gameObject.SetActive(true);
        healTowerButton.gameObject.SetActive(displaySpawnUnitButton || displayLightDropButton);
        healTowerFullWidthButton.gameObject.SetActive(!displaySpawnUnitButton && !displayLightDropButton);
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
        if (item == HighlightedItem.ABA_UNIT_BTN)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(spawnUnitButton.transform.position);
            Util.poolManager.SpawnItemHighlight(worldPos, new Vector2(0,130));  
        }
    }

    private void OnEnable()
    {
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        EventManager.Structures.onStructureGainHealth += OnStructureGainHealth;
        EventManager.Structures.onStructureLoseHealth += OnStructureLoseHealth;
        EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
        EventManager.Tutorials.onHighlightItem += OnHighlightItem;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        EventManager.Structures.onStructureGainHealth -= OnStructureGainHealth;
        EventManager.Structures.onStructureLoseHealth -= OnStructureLoseHealth;
        EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;        
        EventManager.Tutorials.onHighlightItem -= OnHighlightItem;
    }
}
}