using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{
public class WaitingTutState : TutStateBase
{
    public override void Init (TutState tutState)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            
            InputController.canPressButtons = false;
            InputController.canSpawnTowers = true;

            tutCanvas.tutTextWordAnim.ShakeWord("drought");
            tutCanvas.tutTextWordAnim.ShakeWord("defense");
            EventManager.Tutorials.onTutStateInit?.Invoke(tutState);
        }
    }

    public override TutState OnUpdate(TutState tutState)
    {
        Init(tutState); 
        return tutState;
    }

    private void OnTouchBegan(Vector3 pos)
    {
        if (!isInitialized)
            return;
        
        if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
        {
            tutCanvas.SetEndTutState();
            return;
        }

        if (tutCanvas.currTutorial.IsTapAnywhereRequiredAction())
            tutCanvas.SetLetterRevealState();
    }

    private void OnStructureCreated(Structure structure)
    {
        if (!isInitialized)
            return;

        if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
        {
            tutCanvas.SetEndTutState();
            return;
        }
        
        if (structure.IsAbaTower())
        {
            if (tutCanvas.currTutorial.IsPlaceAbaTowerRequiredAction())
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
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        EventManager.Tutorials.onTutStateInit += OnTutStateInit;
        EventManager.Input.onTouchBegan += OnTouchBegan;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        EventManager.Tutorials.onTutStateInit -= OnTutStateInit;
        EventManager.Input.onTouchBegan -= OnTouchBegan;
    }
}
}