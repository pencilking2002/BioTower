using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class NoneTutState : TutStateBase
{
    public override void Init (TutState tutState)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            
            InputController.canPressButtons = true;
            InputController.canSpawnTowers = true;

            EventManager.Tutorials.onTutStateInit?.Invoke(tutState);
        }
    }

    public override TutState OnUpdate(TutState tutState)
    {
        Init(tutState);

        if (tutCanvas.hasTutorials)
        {
            tutCanvas.initTutPanelLocalPos = tutCanvas.tutPanel.localPosition;
            tutCanvas.tutPanel.localPosition += new Vector3(0, tutCanvas.slideInOffset, 0);

            tutState = TutState.LETTER_REVEAL;
        }
        else if (tutCanvas.canvas.enabled)
        {
            tutCanvas.canvas.enabled = false;
        }

        return tutState;
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