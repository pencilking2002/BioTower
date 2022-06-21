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
        [SerializeField] private SpriteRenderer glow;

        private void Awake()
        {
            influenceCollider.transform.localScale =
                Vector3.one * Util.upgradeSettings.ppc2ExplosionColliderScale_float.GetFloat();

            glow.transform.localScale =
                Vector3.one * Util.upgradeSettings.ppc2ExplosionSpriteScale_float.GetFloat();
        }

        public void Explode()
        {
            if (!influenceCollider)
                return;

            float radius = influenceCollider.radius * influenceCollider.transform.localScale.x;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, GameManager.Instance.util.enemyLayerMask);

            if (enemies.Length != 0)
            {
                foreach (Collider2D enemyCol in enemies)
                {
                    var enemy = enemyCol.transform.parent.GetComponent<EnemyUnit>();
                    enemy.TakeDamage(Util.gameSettings.upgradeSettings.ppc2TowerDamage);
                }
            }

            var explosion = Instantiate(explosiionPrefab);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}