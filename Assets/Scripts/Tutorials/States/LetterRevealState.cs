using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class LetterRevealState : TutStateBase
{
    public static bool cancelLetterReveal;

    public override void Init (TutState tutState)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            tutCanvas.currTutorialIndex++;
       
            if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
            {
                tutCanvas.SetEndTutState();
                return;
            }

            tutCanvas.portraitController.SetPortrait(tutCanvas.currTutorial.portraitIndex);
            
            if (tutCanvas.currTutorial.transition == TransitionType.SLIDE_IN)
                SlideIn();
            else if (tutCanvas.currTutorial.transition == TransitionType.BLINK)
                Blink();

            EventManager.Tutorials.onTutStateInit?.Invoke(tutState);
        }
    }

    public override TutState OnUpdate(TutState tutState)
    {
        Init(tutState);
        return tutState;
    }

    private void SlideIn()
    {
        tutCanvas.tutText.text = "";
        var seq = LeanTween.sequence();
        seq.append(tutCanvas.currTutorial.delay);
        seq.append(() => { EventManager.Tutorials.onTutTextPopUp?.Invoke(); });
        seq.append(LeanTween.moveLocalY(tutCanvas.tutPanel.gameObject, tutCanvas.initTutPanelLocalPos.y, 0.25f).setEaseOutCubic());
        seq.append(() => {
            tutCanvas.tutText.text = tutCanvas.currTutorial.text;
            GameManager.Instance.util.TextReveal(tutCanvas.tutText, tutCanvas.revealDuration, SetWaitingState);

        //     if (currTutorial.hasArrows)
        //         arrowController.DisplayArrows(currTutorial.arrowCoords);
        });
    }

    private void Blink()
    {
        EventManager.Tutorials.onTutTextPopUp?.Invoke();
        LeanTween.scale(tutCanvas.tutText.gameObject, Vector3.one * 1.1f, 0.05f).setLoopPingPong(1).setOnComplete(() => {
            tutCanvas.tutText.text = tutCanvas.currTutorial.text;
            GameManager.Instance.util.TextReveal(tutCanvas.tutText, tutCanvas.revealDuration, SetWaitingState);
            
            // if (currTutorial.hasArrows)
            //     arrowController.DisplayArrows(currTutorial.arrowCoords);                    
        });
    }

    private void SetWaitingState()
    {
        if (!isInitialized)
            return;

        bool gotoWaitState = 
            tutCanvas.currTutorial.IsTapAnywhereRequiredAction() ||
            tutCanvas.currTutorial.IsPlaceAbaTowerRequiredAction() ||
            tutCanvas.currTutorial.IsTowerSelectedRequiredAction();
        
        bool gotoWaitButtonState = 
            tutCanvas.currTutorial.IsTapAbaButtonRequiredAction() ||
            tutCanvas.currTutorial.IsSpawnAbaUnitRequiredAction();

        if (gotoWaitState)
        {
            tutCanvas.SetWaitingTapState();
        }
        else if (gotoWaitButtonState)
        {
            tutCanvas.SetWaitingButtonTapState();
        }
    }

    public override void OnTutStateInit(TutState tutState)
    {
        if (tutState != this.tutState)
            isInitialized = false;
    }

    private void OnTouchBegan(Vector3 pos)
    {
        if (!isInitialized)
            return;
        
        cancelLetterReveal = true;
        SetWaitingState();
    }

    private void OnEnable()
    {
        EventManager.Tutorials.onTutStateInit += OnTutStateInit;
        EventManager.Input.onTouchBegan += OnTouchBegan;
    }

    private void OnDisable()
    {
        EventManager.Tutorials.onTutStateInit -= OnTutStateInit;
        EventManager.Input.onTouchBegan -= OnTouchBegan;
    }
}
}