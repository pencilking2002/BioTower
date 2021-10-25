using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace BioTower
{
public enum RequiredAction
{
    TAP_ANYWHERE, TAP_ABA_TOWER_BUTTON,
    PLACE_ABA_TOWER, TOWER_SELECTED,
    SPAWN_ABA_UNIT
}

public enum TransitionType
{
    SLIDE_IN, BLINK
}

public enum HighlightedItem
{
    NONE, ABA_TOWER_BTN, 
    ABA_UNIT_BTN, SOCKET, ENERGY,
}

[CreateAssetMenu(menuName ="BioTower/TutorialData", fileName ="TutorialData")]
public class TutorialData : ScriptableObject
{
    [Multiline(10)] public string text;
    public AnimatedWord[] animatedWords;
    [Range(0,3)] public int portraitIndex;
    public RequiredAction requiredAction;
    public TransitionType transition;
    [Range(0,5)] public float delay;
    public HighlightedItem highlightedItem;
    
    public bool IsTapAnywhereRequiredAction() { return requiredAction == RequiredAction.TAP_ANYWHERE; }
    public bool IsTapAbaButtonRequiredAction() { return requiredAction == RequiredAction.TAP_ABA_TOWER_BUTTON; }
    public bool IsPlaceAbaTowerRequiredAction() { return requiredAction == RequiredAction.PLACE_ABA_TOWER; }
    public bool IsTowerSelectedRequiredAction() { return requiredAction == RequiredAction.TOWER_SELECTED; }
    public bool IsSpawnAbaUnitRequiredAction() { return requiredAction == RequiredAction.SPAWN_ABA_UNIT; }

}

[Serializable]
public class AnimatedWord
{
    public string word;
    public Vector2 speed = new Vector2(100,10);
    public float amplitude = 0.2f;
}
}