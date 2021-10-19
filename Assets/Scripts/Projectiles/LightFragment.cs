﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class LightFragment : MonoBehaviour
{
    public bool hasBeenPickedUp;

    public void DestroyObject()
    {
        if (hasBeenPickedUp)
            return;

        GameManager.Instance.CreateLightExplosion(transform.position);
        hasBeenPickedUp = true;
        EventManager.Structures.onLightPickedUp?.Invoke();

        var scale = transform.localScale;

        var seq = LeanTween.sequence();

        seq.append(LeanTween.scale(gameObject, scale * 3f, 0.1f));

        seq.append(LeanTween.scale(gameObject, Vector3.zero, 0.1f));

        seq.append(gameObject, () => {
            GetComponent<PooledObject>().SendToPool();
            hasBeenPickedUp = false;
        });
    }
}
}