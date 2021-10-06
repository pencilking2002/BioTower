using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using BioTower.Structures;
using BioTower.Units;
using Sirenix.OdinInspector;

namespace BioTower
{
public class TutorialCanvas : MonoBehaviour
{
    public static bool tutorialInProgress;
    private Dictionary<TutState, TutStateBase> charStates = new Dictionary<TutState, TutStateBase>();

    public TutState tutState;

    [Header("Tutorial Data")]
    public bool hasTutorials;
    [ShowIf("hasTutorials")][SerializeField] private TutorialData[] tutorials;
    [ShowIf("hasTutorials")][SerializeField] private int currTutorialIndex = -1;
    public TutorialData currTutorial => tutorials[currTutorialIndex];


    public bool IsLastTutorial(TutorialData data)
    {
        int index = Array.IndexOf(tutorials, data);
        return index == (tutorials.Length-1);
    }

    private void Awake()
    {
        CacheStates();
    }

    private void CacheStates()
    {
        var states = GetComponentsInChildren<TutStateBase>();
        foreach(TutStateBase state in states)
        {
            charStates.Add(state.tutState, state);
        }
    } 

    private void Update()
    {
        if (!GameManager.Instance.gameStates.IsGameState())
            return;

        tutState = charStates[tutState].OnUpdate(tutState);
    }

    public void SetNoneState() { tutState = TutState.NONE; }
    public void SetLetterRevealState() { tutState = TutState.LETTER_REVEAL; }
    public void SetWaitingTapState() { tutState = TutState.WAITING_TAP; }
    public void SetWaitingButtonTapState() { tutState = TutState.WAITING_BUTTON_TAP; }

    public bool IsNoneState() { return tutState == TutState.NONE; }
    public bool IsLetterRevealState() { return tutState == TutState.LETTER_REVEAL; }
    public bool IsWaitingTapState() { return tutState == TutState.WAITING_TAP; }
    public bool IsWaitingButtonTapState() { return tutState == TutState.WAITING_BUTTON_TAP; }

}
}
