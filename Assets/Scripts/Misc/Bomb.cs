using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using Sirenix.OdinInspector;

namespace BioTower
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private CircleCollider2D radiusCollider;
        [SerializeField] private GameObject explosion;
        [SerializeField] private int damage;
        [Button]
        public void Explode(float delay = 0)
        {
            var seq = LeanTween.sequence();
            seq.append(delay);
            seq.append(() =>
            {
                var radius = radiusCollider.transform.lossyScale.x * 0.5f;
                var colliders = Physics2D.OverlapCircleAll(transform.position, radius, GameManager.Instance.util.enemyLayerMask);
                foreach (Collider2D collider in colliders)
                {
                    // Make enemies take damage
                    collider.transform.parent.GetComponent<EnemyUnit>().TakeDamage(damage);
                }
            });
            seq.append(() => { sr.enabled = false; });
            seq.append(() => { explosion.SetActive(true); });
            seq.append(1.0f);
            seq.append(() => { gameObject.SetActive(false); });

        }
    }
}
