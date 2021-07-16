using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private float scaleAnimDuration = 0.1f;
    [SerializeField] private Text currTowerText;

    
    [Header("Tower Icons")]
    [SerializeField] private Image towerIcon;
    [SerializeField] private Sprite playerTower;
    [SerializeField] private Sprite abaTower;
    [SerializeField] private Sprite pp2cTower;
    [SerializeField] private Sprite chloroplastTower;
    private Dictionary<StructureType, Sprite> iconMap = new Dictionary<StructureType, Sprite>();

    private void Awake()
    {
        towerPanel.gameObject.SetActive(false);
        iconMap.Add(StructureType.ABA_TOWER, abaTower);
        iconMap.Add(StructureType.PPC2_TOWER, pp2cTower);
        iconMap.Add(StructureType.CHLOROPLAST, chloroplastTower);
        iconMap.Add(StructureType.DNA_BASE, playerTower);
    }

    public void OnPressSpawnUnitButton()
    {
        if (GameManager.Instance.econManager.CanBuyAbaUnit())
        {
            GameManager.Instance.econManager.BuyAbaUnit();
            GameManager.Instance.tapManager.selectedStructure.SpawnUnits(1);
        }
    }

    public void OnPressHealTowerButton()
    {
        if (GameManager.Instance.econManager.CanBuyTowerHeal())
        {
            var healAmount = GameManager.Instance.gameSettings.healTowerAmount;
            GameManager.Instance.tapManager.selectedStructure.GainHealth(healAmount);
            GameManager.Instance.econManager.BuyTowerHeal();
            var selectedTower = GameManager.Instance.tapManager.selectedStructure;
            Util.ScaleBounceSprite(selectedTower.sr, 1.1f);
            var oldColor = selectedTower.sr.color;
            selectedTower.sr.color = Color.green;
            LeanTween.value(selectedTower.gameObject, selectedTower.sr.color, oldColor, 0.25f)
            .setOnUpdate((Color col) => {
                selectedTower.sr.color = col;
            });
        }
    }

    public void OnStructureSelected(Structure structure)
    {
        //Debug.Log($"Tap {tower.structureType}");
        bool displaySpawnUnitButton = structure.structureType == StructureType.ABA_TOWER;
        spawnUnitButton.gameObject.SetActive(displaySpawnUnitButton);
        currTowerText.text = structure.structureType.ToString().Replace('_', ' ');
        towerPanel.gameObject.SetActive(true);
        healTowerButton.gameObject.SetActive(displaySpawnUnitButton);
        healTowerFullWidthButton.gameObject.SetActive(!displaySpawnUnitButton);
        UpdateTowerHealthBar(structure);
        towerIcon.sprite = iconMap[structure.structureType];
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

    private void OnEnable()
    {
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        EventManager.Structures.onStructureGainHealth += OnStructureGainHealth;
        EventManager.Structures.onStructureLoseHealth += OnStructureLoseHealth;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        EventManager.Structures.onStructureGainHealth -= OnStructureGainHealth;
        EventManager.Structures.onStructureLoseHealth -= OnStructureLoseHealth;
    }
}
}