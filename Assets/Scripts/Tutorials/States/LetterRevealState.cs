using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BioTower
{
public class LetterRevealState : TutStateBase
{
    public static bool cancelLetterReveal;
    private bool isTutAnimating;

    public override void Init (TutState tutState)
    {
        if (!isInitialized)
        {
            isInitialized = true;

            InputController.canPressButtons = false;
            InputController.canSpawnTowers = false;

            tutCanvas.currTutorialIndex++;
                
            SetupCtaText();

            tutCanvas.portraitController.SetPortrait(tutCanvas.currTutorial.portraitIndex);
            
            // Animate the tutorial panel
            if (tutCanvas.currTutorial.transition == TransitionType.SLIDE_IN)
                SlideIn();
            else if (tutCanvas.currTutorial.transition == TransitionType.BLINK)
                Blink();

            EventManager.Tutorials.onTutStateInit?.Invoke(tutState);
            Util.poolManager.DespawnAllitemHighlights();
            EventManager.Tutorials.onHighlightItem?.Invoke(tutCanvas.currTutorial.highlightedItem);
        }
    }

    public override TutState OnUpdate(TutState tutState)
    {
        Init(tutState);

        // if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
        //     tutState = TutState.END;
    
        return tutState;
    }

    private void SetupCtaText()
    {
        LeanTween.cancel(tutCanvas.ctaText.gameObject);
        tutCanvas.ctaText.alpha = 0;

        if (tutCanvas.currTutorial.requiredAction == RequiredAction.TAP_ANYWHERE)
        {
            var text = tutCanvas.ctaText.GetComponent<TextMeshProUGUI>();
            if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
                text.text = "DONE";
            else
                text.text = "NEXT";

            LeanTween.delayedCall(tutCanvas.ctaText.gameObject, 1.0f, () => {
                LeanTween.alphaCanvas(tutCanvas.ctaText, 1, 0.5f).setLoopPingPong(-1);
            });
        }
    }

    private void SlideIn()
    {
        isTutAnimating = true;
        tutCanvas.tutText.text = "";
        var seq = LeanTween.sequence();
        seq.append(tutCanvas.currTutorial.delay);
        seq.append(() => { EventManager.Tutorials.onTutTextPopUp?.Invoke(); });
        seq.append(LeanTween.moveLocalY(tutCanvas.tutPanel.gameObject, tutCanvas.initTutPanelLocalPos.y, 0.25f).setEaseOutCubic());
        seq.append(() => {
            isTutAnimating = false;
            tutCanvas.tutText.text = tutCanvas.currTutorial.text;
            GameManager.Instance.util.TextReveal(tutCanvas.tutText, tutCanvas.revealDuration, SetWaitingState);

        //     if (currTutorial.hasArrows)
        //         arrowController.DisplayArrows(currTutorial.arrowCoords);
        });
    }

    private void Blink()
    {
        isTutAnimating = true;
        EventManager.Tutorials.onTutTextPopUp?.Invoke();
        LeanTween.scale(tutCanvas.tutText.gameObject, Vector3.one * 1.1f, 0.05f).setLoopPingPong(1).setOnComplete(() => {
            isTutAnimating = false;
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
        
        if (isTutAnimating)
            return;

        cancelLetterReveal = true;

        LeanTween.delayedCall(1.0f, SetWaitingState);
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