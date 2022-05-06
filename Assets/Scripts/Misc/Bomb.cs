using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D radiusCollider;
        public void Explode()
        {
            var radius = radiusCollider.transform.lossyScale.x * 0.5f;
            var colliders = Physics2D.OverlapCircleAll(transform.position, radius, GameManager.Instance.util.enemyLayerMask);
            foreach (Collider2D collider in colliders)
            {

            }
        }
    }
}
