using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{

public enum SocketType
{
    DEFAULT,
    SPECIAL
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

    private void Start()
    {
        EventManager.Structures.onSocketStart?.Invoke(this);
    }

    private void OnStartPlacementState(StructureType structureType)
    {
        if (hasStructure)
            return;

        bool isSpecial = socketType == SocketType.SPECIAL && (structureType == StructureType.CHLOROPLAST || structureType == StructureType.MITOCHONDRIA);
        bool isDefault = socketType == SocketType.DEFAULT && (structureType != StructureType.CHLOROPLAST && structureType != StructureType.MITOCHONDRIA);

        if (isSpecial)
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
        bool isSpecial = socketType == SocketType.SPECIAL && (structureType == StructureType.CHLOROPLAST || structureType == StructureType.MITOCHONDRIA);
        bool isDefault = socketType == SocketType.DEFAULT && (structureType != StructureType.CHLOROPLAST && structureType != StructureType.MITOCHONDRIA);
        return isSpecial || isDefault;
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

    private void OnHighlightItem(HighlightedItem item)
    {
        if (item == HighlightedItem.NONE || item != HighlightedItem.SOCKET)
            return;
    }

    private void OnEnable()
    {
        EventManager.Structures.onStartPlacementState += OnStartPlacementState;
        EventManager.Structures.onSetNonePlacementState += OnSetNonePlacementState;
        EventManager.Tutorials.onHighlightItem += OnHighlightItem;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStartPlacementState -= OnStartPlacementState;
        EventManager.Structures.onSetNonePlacementState -= OnSetNonePlacementState;
        EventManager.Tutorials.onHighlightItem -= OnHighlightItem;
    }
}
}