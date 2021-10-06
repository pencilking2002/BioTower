using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class WaitingTutState : TutStateBase
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