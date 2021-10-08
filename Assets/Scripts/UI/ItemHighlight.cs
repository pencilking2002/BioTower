using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHighlight : MonoBehaviour
{
    public void Oscillate()
    {
        Stop();
        var arrowPos = this.transform.localPosition;
        LeanTween.moveLocalY(gameObject, arrowPos.y - 20f, 0.5f).setLoopPingPong(-1);
    }

    public void Stop()
    {
        LeanTween.cancel(gameObject);
    }
}
