using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{
public class TowerMenu : MonoBehaviour
{
    [SerializeField] private RectTransform towerPanel;
    [SerializeField] private float scaleAnimDuration = 0.1f;

    private void Awake()
    {
        //canvas = GetComponent<Canvas>();
        //tower = GetComponentInParent<Structure>();
        //tower.towerMenu = this;
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
        }
    }

    public void OnStructureSelected(Structure structure)
    {
        //Debug.Log($"Tap {tower.structureType}");
        towerPanel.gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one * 0.5f, scaleAnimDuration);
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
        EventManager.Structures.onStructureCreated += OnStructureCreated;
    }
}
}