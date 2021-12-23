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
            StopCoroutine(GameManager.Instance.util.RevealCharacters(null, 0, null));
            tutCanvas.tutText.text = tutCanvas.currTutorial.text;
            tutCanvas.tutText.ForceMeshUpdate();
            
            InputController.canPressButtons = false;
            InputController.canSpawnTowers = true;
            EventManager.Tutorials.onTutStateInit?.Invoke(tutState);

            var animatedWords = tutCanvas.currTutorial.animatedWords;
            foreach(AnimatedWord word in animatedWords)
                EventManager.Tutorials.onAnimateText?.Invoke(word.word, word.speed, word.amplitude);
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
        
        if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial) && !tutCanvas.currTutorial.IsTapLightDropRequiredAction())
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

        if (!structure.IsPlayerBase() && tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
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
    private void OnLightPickedUp()
    {
        if (!isInitialized)
            return;

        if (Util.tutCanvas.hasTutorials && TutorialCanvas.tutorialInProgress)
        {
            var currTut = Util.tutCanvas.currTutorial;
            var isValid = currTut.highlightedItem == HighlightedItem.MINI_CHLORO && currTut.highlightType == HighlightType.ARROW;
            if (isValid)
            {
                if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
                {
                    //Debug.Log("last tut");
                    tutCanvas.SetEndTutState();
                }
                else
                {
                    tutCanvas.SetLetterRevealState();
                    //Debug.Log("Go to text reveal");
                }
    
                Util.poolManager.DespawnAllitemHighlights();
            }
        }
    }
    
    private void OnEnable()
    {
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        EventManager.Tutorials.onTutStateInit += OnTutStateInit;
        EventManager.Input.onTouchBegan += OnTouchBegan;
        EventManager.Structures.onLightPickedUp += OnLightPickedUp;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        EventManager.Tutorials.onTutStateInit -= OnTutStateInit;
        EventManager.Input.onTouchBegan -= OnTouchBegan;
        EventManager.Structures.onLightPickedUp -= OnLightPickedUp;
    }
}
}