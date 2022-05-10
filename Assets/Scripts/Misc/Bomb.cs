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
        [SerializeField] private float explosionDelay = 0.5f;
        [SerializeField] private float totalBlinkDuration = 1.0f;
        [SerializeField] private int numBlinks = 3;
        private bool explosionStarted;


        [Button]
        public void Explode()
        {
            if (explosionStarted)
                return;

            explosionStarted = true;
            float blinkDuration = totalBlinkDuration / numBlinks;
            var initColor = sr.color;

            var seq = LeanTween.sequence();

            seq.append(explosionDelay);

            for (int i = 0; i < numBlinks; i++)
            {
                seq.append(() => { sr.color = Color.white; });
                seq.append(blinkDuration);
                seq.append(() => { sr.color = initColor; });
                seq.append(blinkDuration);
            }

            //seq.append(delay);
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

        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.layer == Util.enemyLayer)
            {
                Explode();
            }
        }
    }
}
