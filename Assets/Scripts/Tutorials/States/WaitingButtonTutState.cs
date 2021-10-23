using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using BioTower.Units;

namespace BioTower
{
public class WaitingButtonTutState : TutStateBase
{
    public override void Init (TutState tutState)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            
            InputController.canPressButtons = true;
            InputController.canSpawnTowers = false;
            // tutCanvas.tutTextWordAnim.ShakeWord("drought");
            // tutCanvas.tutTextWordAnim.ShakeWord("defense");
            EventManager.Tutorials.onTutStateInit?.Invoke(tutState);
            EventManager.Tutorials.onAnimateText?.Invoke("drought", new Vector2(100,10), 0.2f);
            EventManager.Tutorials.onAnimateText?.Invoke("defense", new Vector2(100,10), 0.2f);
        }
    }

    public override TutState OnUpdate(TutState tutState)
    {
        Init(tutState);
        return tutState;
    }

    private void OnPressTowerButton(StructureType structureType)
    {
        if (!isInitialized)
            return;

        if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
        {
            tutCanvas.SetEndTutState();
            return;
        }
                
        if (structureType == StructureType.ABA_TOWER)
        {
            if (tutCanvas.currTutorial.requiredAction == RequiredAction.TAP_ABA_TOWER_BUTTON)
            {
                tutCanvas.SetLetterRevealState();
            }
        }
    }

    private void OnTapSpawnUnitButton(UnitType unitType)
    {
        if (!isInitialized)
            return;

        if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
        {
            tutCanvas.SetEndTutState();
            return;
        }

        if (unitType == UnitType.ABA)
        {
            if (tutCanvas.currTutorial.IsSpawnAbaUnitRequiredAction())
            {
                tutCanvas.SetLetterRevealState();
            }
        }
    }


    public override void OnTutStateInit(TutState tutState)
    {
        if (tutState != this.tutState)
            isInitialized = false;
    }

    private void OnEnable()
    {
        EventManager.Tutorials.onTutStateInit += OnTutStateInit;
        EventManager.UI.onPressTowerButton += OnPressTowerButton;
        EventManager.UI.onTapSpawnUnitButton += OnTapSpawnUnitButton;
    }

    private void OnDisable()
    {
        EventManager.Tutorials.onTutStateInit -= OnTutStateInit;
        EventManager.UI.onPressTowerButton += OnPressTowerButton;
        EventManager.UI.onTapSpawnUnitButton -= OnTapSpawnUnitButton;
    }
}
}