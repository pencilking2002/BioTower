using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public static Action onWavesCompleted;
    protected WaveManager waveManager => GameManager.Instance.waveManager;

    public virtual WaveMode OnUpdate(Wave wave)
    {
        return wave.state;
    }
}
}
