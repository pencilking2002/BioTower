using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float delay;
    private void Awake()
    {
        LeanTween.delayedCall(gameObject, delay, () => {
            Destroy(gameObject);
        });
    }
}
}