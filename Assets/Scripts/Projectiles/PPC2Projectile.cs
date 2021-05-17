using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPC2Projectile : MonoBehaviour
{
    [SerializeField] private GameObject explosiionPrefab;

    public void Explode()
    {
        var explosion = Instantiate(explosiionPrefab);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}
