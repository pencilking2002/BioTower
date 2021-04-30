using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{
    
[SelectionBase]
public class StructureSocket : MonoBehaviour
{
    [SerializeField] private bool hasStructure;
    [SerializeField] private SpriteRenderer glowingSprite;
    [SerializeField] private Color glowColor;
    [SerializeField] private float glowAnimDuration = 0.5f;
    private Color defaultColor;

    private void Awake()
    {
        defaultColor = glowingSprite.color;
    }

    private void OnStartPlacementState(StructureType structureType)
    {
        if (hasStructure)
            return;

        var currColor = glowingSprite.color;
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, currColor, glowColor, glowAnimDuration).setOnUpdate((Color col) => {
            glowingSprite.color = col;
        }).setLoopPingPong(-1);
    }

    public void SetHasStructure(bool hasStructure)
    {
        this.hasStructure = hasStructure; 
    }

    public bool HasStructure()
    {
        return hasStructure;
    }

    private void OnSetNonePlacementState()
    {
        LeanTween.cancel(gameObject);
        glowingSprite.color = defaultColor;
    }

    private void OnEnable()
    {
        PlacementManager.onStartPlacementState += OnStartPlacementState;
        PlacementManager.onSetNonePlacementState += OnSetNonePlacementState;
    }

    private void OnDisable()
    {
        PlacementManager.onStartPlacementState -= OnStartPlacementState;
        PlacementManager.onSetNonePlacementState -= OnSetNonePlacementState;
    }
}
}