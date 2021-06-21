using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{

public enum SocketType
{
    DEFAULT,
    CHLOROPLAST
}

[SelectionBase]
public class StructureSocket : MonoBehaviour
{
    [SerializeField] private bool hasStructure;
    [SerializeField] private SocketType socketType;
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

        bool isChloroplast = socketType == SocketType.CHLOROPLAST && structureType == StructureType.CHLOROPLAST;
        bool isDefault = socketType == SocketType.DEFAULT && structureType != StructureType.CHLOROPLAST;

        if (isChloroplast)
        {
            var currColor = glowingSprite.color;
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, currColor, glowColor, glowAnimDuration).setOnUpdate((Color col) => {
                glowingSprite.color = col;
            }).setLoopPingPong(-1);
        }
        else if (isDefault)
        {
            var currColor = glowingSprite.color;
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, currColor, glowColor, glowAnimDuration).setOnUpdate((Color col) => {
                glowingSprite.color = col;
            }).setLoopPingPong(-1);
        }
    }

    public bool CanAcceptStructure(StructureType structureType)
    {
        bool isChloroplast = socketType == SocketType.CHLOROPLAST && structureType == StructureType.CHLOROPLAST;
        bool isDefault = socketType == SocketType.DEFAULT && structureType != StructureType.CHLOROPLAST;
        return isChloroplast || isDefault;
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
        EventManager.Structures.onStartPlacementState += OnStartPlacementState;
        EventManager.Structures.onSetNonePlacementState += OnSetNonePlacementState;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStartPlacementState -= OnStartPlacementState;
        EventManager.Structures.onSetNonePlacementState -= OnSetNonePlacementState;
    }
}
}