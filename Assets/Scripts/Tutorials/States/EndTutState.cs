using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower
{
public class EndTutState : TutStateBase
{
    public override void Init (TutState tutState)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            TutorialCanvas.tutorialInProgress = false;
            InputController.canPressButtons = true;
            InputController.canSpawnTowers = true;
            
            var seq = LeanTween.sequence();
            seq.append(LeanTween.moveLocalY(tutCanvas.tutPanel.gameObject, tutCanvas.initTutPanelLocalPos.y+tutCanvas.slideInOffset, 0.25f).setEaseOutCubic());
            seq.append(() => {
                EventManager.Tutorials.onTutorialEnd?.Invoke(tutCanvas.currTutorial);
                tutCanvas.hasTutorials = false;
            });
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
    }

    public override void OnTutStateInit(TutState tutState)
    {
        if (tutState != this.tutState)
            isInitialized = false;
    }

    
    private void OnEnable()
    {
        EventManager.Tutorials.onTutStateInit += OnTutStateInit;
    }

    private void OnDisable()
    {
        EventManager.Tutorials.onTutStateInit -= OnTutStateInit;
    }
}
}