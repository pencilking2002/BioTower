using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class PPC2Projectile : MonoBehaviour
{
    [SerializeField] private GameObject explosiionPrefab;
    [SerializeField] private CircleCollider2D influenceCollider;

    public void Explode()
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, influenceCollider.radius, GameManager.Instance.util.enemyLayerMask);

        foreach(Collider2D enemyCollider in enemies)
        {
            Debug.Log("Hit enemy");
        }

        var explosion = Instantiate(explosiionPrefab);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}
}