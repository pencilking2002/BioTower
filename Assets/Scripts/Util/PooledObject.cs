using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    public class PooledObject : MonoBehaviour
    {
        public PoolObjectType objectType;
        //public bool isActive = true;

        public void SendToPool()
        {
            Util.poolManager.AddPooledObject(this);
        }

        public bool IsWaveWarning()
        {
            return objectType == PoolObjectType.WAVE_WARNING;
        }
        public WaveWarning GetWaveWarning()
        {
            return GetComponent<WaveWarning>();
        }

        public bool IsLightFragment()
        {
            return objectType == PoolObjectType.LIGHT_FRAGMENT;
        }

        public LightFragment GetLightFragment()
        {
            return GetComponent<LightFragment>();
        }

    }
}