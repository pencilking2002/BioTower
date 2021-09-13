using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;

namespace BioTower
{
public class PPC2Projectile : MonoBehaviour
{
    [SerializeField] private GameObject explosiionPrefab;
    [SerializeField] private CircleCollider2D influenceCollider;

    public void Explode()
    {
        var enemyGO = Physics2D.OverlapCircle(transform.position, influenceCollider.radius, GameManager.Instance.util.enemyLayerMask);

        if (enemyGO != null)
        {
            var enemy = enemyGO.transform.parent.GetComponent<BasicEnemy>();
            enemy.TakeDamage(Util.gameSettings.upgradeSettings.ppc2Damage);
        }

        var explosion = Instantiate(explosiionPrefab);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}
}