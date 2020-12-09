using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class NotStartedState : WaveState
{
    public override WaveMode OnUpdate(Wave wave)
    {
        wave.timeStarted = Time.time;
        return WaveMode.DELAY;
    }
}
}
