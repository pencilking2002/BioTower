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
            EventManager.Tutorials.onTutStateInit?.Invoke(tutState);
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