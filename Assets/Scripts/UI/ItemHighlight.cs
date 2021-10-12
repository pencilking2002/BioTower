using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemHighlight : MonoBehaviour
{
    public void Oscillate(Action onComplete=null)
    {
        Stop();
        var arrowPos = this.transform.localPosition;
        this.transform.localScale = Vector3.zero;
        
        LeanTween.moveLocalY(gameObject, arrowPos.y - 20f, 0.5f).setLoopPingPong(-1);
        LeanTween.scale(gameObject, Vector3.one, 0.25f).setOnComplete(() => {
            onComplete?.Invoke();
        });
    }

    public void Stop(Action onComplete=null)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, 0.25f).setOnComplete(() => {
            onComplete?.Invoke();
        });
    }
}
