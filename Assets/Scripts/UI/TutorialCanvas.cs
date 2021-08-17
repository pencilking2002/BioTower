﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using BioTower.Structures;
using BioTower.Units;

namespace BioTower
{
public class TutorialCanvas : MonoBehaviour
{
    public static bool tutorialInProgress;

    [Header("Tutorial Data")]
    [SerializeField] private TutorialData[] tutorials;
    [SerializeField] private int currTutorialIndex = -1;
    [SerializeField] private bool initTutorialOnStart;
    public TutorialData currTutorial => tutorials[currTutorialIndex];


    [Header("Tutorial UI")]
    public Image tutPanel;
    [SerializeField] private ArrowController arrowController;
    [SerializeField] private TextMeshProUGUI tutText;
    [SerializeField] private CanvasGroup ctaText;
    [SerializeField] private int slideInOffset = 50;
    [SerializeField] private float revealDuration = 0.05f;
    private Vector3 initTutPanelLocalPos;

    private void Start()
    {
        initTutPanelLocalPos = tutPanel.transform.localPosition;

        if (initTutorialOnStart)
            StartNextTutorial();
        
    }

    public void StartNextTutorial()
    {
        if (currTutorialIndex+1 >= tutorials.Length)
        {
            EndTutorial();
        }
        else
        {
            LeanTween.cancel(ctaText.gameObject);
            ctaText.alpha = 0;
            tutorialInProgress = true;
            
            currTutorialIndex++;
            
            
            //if (!currTutorial.hasArrow)
            arrowController.HideArrows();

            if (currTutorial.transition == TransitionType.SLIDE_IN)
            {
              
                tutPanel.transform.localPosition += new Vector3(0, slideInOffset, 0);
                var seq = LeanTween.sequence();
                seq.append(currTutorial.delay);
                seq.append(LeanTween.moveLocalY(tutPanel.gameObject, initTutPanelLocalPos.y, 0.25f).setEaseOutCubic());
                seq.append(() => {
                    tutText.text = currTutorial.text;
                    GameManager.Instance.util.TextReveal(tutText, revealDuration);
                    if (currTutorial.hasArrows)
                        arrowController.DisplayArrows(currTutorial.arrowCoords);
                });

            }
            else if (currTutorial.transition == TransitionType.BLINK)
            {
                LeanTween.scale(tutPanel.gameObject, Vector3.one * 1.1f, 0.05f).setLoopPingPong(1).setOnComplete(() => {
                    tutText.text = currTutorial.text;
                    GameManager.Instance.util.TextReveal(tutText, revealDuration);
                    
                    if (currTutorial.hasArrows)
                        arrowController.DisplayArrows(currTutorial.arrowCoords);
                });
            }

            // Blink the conitnue button
            if (currTutorial.requiredAction == RequiredAction.TAP_ANYWHERE)
            {
                if (currTutorialIndex == tutorials.Length-1)
                    ctaText.GetComponent<TextMeshProUGUI>().text = "DONE";
                else
                    ctaText.GetComponent<TextMeshProUGUI>().text = "CONTINUE >";

                LeanTween.delayedCall(ctaText.gameObject, 1.0f, () => {
                    LeanTween.alphaCanvas(ctaText, 1, 0.5f).setLoopPingPong(-1);
                });
            }
        
            EventManager.Tutorials.onTutorialStart?.Invoke(tutorials[currTutorialIndex]);
        }
    }
    private void EndTutorial()
    {
        tutorialInProgress = false;
        var seq = LeanTween.sequence();
        seq.append(LeanTween.moveLocalY(tutPanel.gameObject, initTutPanelLocalPos.y+slideInOffset, 0.25f).setEaseOutCubic());
    }

    private void OnTouchBegan(Vector3 pos)
    {
        if (tutorialInProgress && currTutorial.requiredAction == RequiredAction.TAP_ANYWHERE)
        {
            StartNextTutorial();
        }
    }

    private void OnPressTowerButton(StructureType structureType)
    {
        if (tutorialInProgress && structureType == StructureType.ABA_TOWER)
        {
            if (currTutorial.requiredAction == RequiredAction.TAP_ABA_TOWER_BUTTON)
            {
                StartNextTutorial();
            }
        }
    }

    private void OnTapSpawnUnitButton(UnitType unitType)
    {
        if (tutorialInProgress && unitType == UnitType.ABA)
        {
            if (currTutorial.requiredAction == RequiredAction.SPAWN_ABA_UNIT)
            {
                StartNextTutorial();
            }
        }
    }

    private void OnStructureSelected(Structure structure)
    {
        // if (tutorialInProgress && structure.structureType == StructureType.ABA_TOWER)
        // {
        //     if (currTutorial.requiredAction == RequiredAction.TOWER_SELECTED)
        //     {
        //         StartNextTutorial();
        //     }
        // }
    }

    private void OnStructureCreated(Structure structure)
    {
        if (tutorialInProgress && structure.structureType == StructureType.ABA_TOWER)
        {
            if (currTutorial.requiredAction == RequiredAction.PLACE_ABA_TOWER)
            {
                StartNextTutorial();
            }
        }
    }

    private void OnEnable()
    {
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.UI.onTapSpawnUnitButton += OnTapSpawnUnitButton;
        EventManager.UI.onPressTowerButton += OnPressTowerButton;
        EventManager.Input.onTouchBegan += OnTouchBegan;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.UI.onTapSpawnUnitButton -= OnTapSpawnUnitButton;
        EventManager.Input.onTouchBegan -= OnTouchBegan;
        EventManager.UI.onPressTowerButton -= OnPressTowerButton;
    }

}
}
