using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BioTower.UI
{
    public class HealthBarTester : MonoBehaviour
    {
        public GameObject healthbarPrefab;
        public int health = 5;

        [Button]
        private void SpawnHealthBar()
        {
            var go = Instantiate(healthbarPrefab);
            var healthBar = go.GetComponent<HealthBar>();
            healthBar.Init(health);
        }
    }
}
