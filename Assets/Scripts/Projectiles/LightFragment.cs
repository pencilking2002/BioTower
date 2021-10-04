using System.Collections;
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
        LeanTween.scale(gameObject, Vector3.zero, 0.1f)
            .setOnComplete(() => {
                GetComponent<PooledObject>().SendToPool();
                hasBeenPickedUp = false;
            });
    }
}
}