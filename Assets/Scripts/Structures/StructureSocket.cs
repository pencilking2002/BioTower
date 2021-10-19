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
        var delay = UnityEngine.Random.Range(0.0f, 1.0f);
        var duration = UnityEngine.Random.Range(0.5f, 1.0f);
        var scale = glowingSprite.transform.localScale;
        var additionalScale = UnityEngine.Random.Range(0.0f, 0.2f);

        var color = glowingSprite.color;
        color.r += UnityEngine.Random.Range(0.0f, 0.1f);
        color.g += UnityEngine.Random.Range(0.0f, 0.1f);
        color.b += UnityEngine.Random.Range(0.0f, 0.1f);

        glowingSprite.color = color;
        glowingSprite.transform.localScale = Vector3.zero;
        LeanTween.delayedCall(gameObject, 1+delay, () => {
            defaultColor = glowingSprite.color;
            LeanTween.scale(glowingSprite.gameObject, scale * (1 + additionalScale), duration).setEaseOutElastic();
        });
    }

    private void Start()
    {
        EventManager.Structures.onSocketStart?.Invoke(this);
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

        LeanTween.delayedCall(0.1f, () => {
            var item = Util.poolManager.SpawnItemHighlight(this.transform.position, new Vector2(0,100));
        });
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
        Util.poolManager.DespawnAllitemHighlights();
    }

    private void OnHighlightItem(HighlightedItem item)
    {
        // if (item == HighlightedItem.SOCKET)
        // {
        //     Util.poolManager.SpawnItemHighlight(this.transform.position, new Vector2(0,100));
        // }
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