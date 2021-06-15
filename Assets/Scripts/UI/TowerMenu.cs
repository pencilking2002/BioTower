using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{
public class TowerMenu : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField] private float scaleAnimDuration = 0.1f;
    private Structure tower;
    [SerializeField] private bool isOpen;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        tower = GetComponentInParent<Structure>();
        tower.towerMenu = this;
        canvas.enabled = false;
    }

    public void OnTapStructure(StructureType structureType, Vector3 screenPoint)
    {
        if (isOpen)
            return;

        Debug.Log($"Tap {tower.structureType}");
        canvas.enabled = true;
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one * 0.5f, scaleAnimDuration);
        isOpen = true;
    }

    public void OnPressSpawnUnitButton()
    {
        if (GameManager.Instance.econManager.CanBuyAbaUnit())
        {
            GameManager.Instance.econManager.BuyAbaUnit();
            tower.SpawnUnits(1);
        }
    }

    public void OnPressHealTowerButton()
    {
        if (GameManager.Instance.econManager.CanBuyTowerHeal())
        {
            var healAmount = GameManager.Instance.gameSettings.healTowerAmount;
            tower.GainHealth(healAmount);
        }
    }

    public void OnPressCloseMenuButton()
    {
        if (isOpen)
        {
            LeanTween.scale(gameObject, Vector3.zero, scaleAnimDuration).setOnComplete(() => {
                canvas.enabled = false;
                isOpen = false;
            });
        }
    }
}
}