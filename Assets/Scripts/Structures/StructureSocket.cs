using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{
public class StructureSocket : MonoBehaviour
{
    [SerializeField] private SpriteRenderer glowingSprite;
    [SerializeField] private Color glowColor;
    [SerializeField] private float glowAnimDuration = 0.5f;

    private void OnStartPlacementState(StructureType structureType)
    {
        var currColor = glowingSprite.color;
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, currColor, glowColor, glowAnimDuration).setOnUpdate((Color col) => {
            glowingSprite.color = col;
        }).setLoopPingPong(-1);
    }

    private void OnSetNonePlacementState()
    {
        LeanTween.cancel(gameObject);
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