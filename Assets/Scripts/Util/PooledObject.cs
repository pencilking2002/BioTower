using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class PooledObject : MonoBehaviour
{
    public PoolObjectType objectType;
    public bool isActive = true;

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