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
    

    [Header("Tut UI")]
    public Transform tutPanel;
    public PortraitController portraitController;
    public TextMeshProUGUI tutText;
    

    [Header("Tut State")]
    public bool hasTutorials;
    [ShowIf("hasTutorials")] public TutState tutState;
    [ShowIf("hasTutorials")]public int currTutorialIndex = -1;
    [ShowIf("hasTutorials")][SerializeField] private TutorialData[] tutorials;
    

    [Header("Tut Animation")]
    public int slideInOffset = 50;
    public float revealDuration = 0.05f;

    public TutorialData currTutorial => tutorials[currTutorialIndex];
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public Vector3 initTutPanelLocalPos;
    private Dictionary<TutState, TutStateBase> charStates = new Dictionary<TutState, TutStateBase>();

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        CacheStates();
    }

    public bool IsLastTutorial(TutorialData data)
    {
        int index = Array.IndexOf(tutorials, data);
        return index == (tutorials.Length-1);
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
