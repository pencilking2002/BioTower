using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BioTower
{
public enum TutState
{
    NONE,
    LETTER_REVEAL,
    WAITING_TAP,
    WAITING_BUTTON_TAP,
    END
}

public abstract class TutStateBase : MonoBehaviour
{
    public TutState tutState;
    protected TutorialCanvas tutCanvas;
    [ReadOnly] [SerializeField] protected bool isInitialized;

    public virtual void Awake() 
    { 
        tutCanvas = GetComponentInParent<TutorialCanvas>();
    }

    public virtual void Init(TutState tutState) { }
    public abstract TutState OnUpdate(TutState tutState);

    public abstract void OnTutStateInit(TutState tutState);

}
}