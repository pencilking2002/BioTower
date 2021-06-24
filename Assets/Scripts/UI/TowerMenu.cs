using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using UnityEngine.UI;

namespace BioTower
{
public class TowerMenu : MonoBehaviour
{
    [SerializeField] private RectTransform towerPanel;
    [SerializeField] private Button healTowerButton;
    [SerializeField] private Button healTowerFullWidthButton;
    [SerializeField] private Button spawnUnitButton;
    [SerializeField] private float scaleAnimDuration = 0.1f;
    [SerializeField] private Text currTowerText;

    private void Awake()
    {
        towerPanel.gameObject.SetActive(false);
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
    }
    
    private void OnStructureCreated(Structure structure)
    {
        if (structure.structureType == StructureType.DNA_BASE)
            return;
            
        OnStructureSelected(structure);
    }

    private void OnEnable()
    {
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.Structures.onStructureCreated += OnStructureCreated;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
    }
}
}