﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrystal : MonoBehaviour
{
    public bool hasBeenPickedUp;

    public void DestroyObject()
    {
        if (hasBeenPickedUp)
            return;

        hasBeenPickedUp = true;
        LeanTween.scale(gameObject, Vector3.zero, 0.1f)
            .setOnComplete(() => {
                Destroy(gameObject);
            });
    }
}
