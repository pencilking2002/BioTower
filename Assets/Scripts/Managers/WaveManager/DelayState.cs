using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class DelayState : WaveState
{
    public override WaveMode OnUpdate(Wave wave)
    {
        if (Time.time > wave.timeStarted + wave.startDelay)
        {
            return WaveMode.IN_PROGRESS;
        }
        else
        {
            return wave.state;
        }
    }
}
}