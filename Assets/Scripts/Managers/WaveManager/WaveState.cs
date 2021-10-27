using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace BioTower
{
public enum WaveMode
{
    NOT_STARTED,
    DELAY,
    IN_PROGRESS,
    ENDED
}

public class WaveState : MonoBehaviour
{
    protected WaveManager waveManager => GameManager.Instance.waveManager;
    [SerializeField] protected WaveMode waveState;
    [ReadOnly][SerializeField] protected bool isInitialized;

    public virtual void Init()
    {

    }

    public virtual WaveMode OnUpdate(WaveMode waveState)
    {
        return waveState;
    }
}
}
