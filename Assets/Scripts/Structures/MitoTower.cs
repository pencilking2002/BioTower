using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

namespace BioTower.Structures
{
    public class MitoTower : Structure
    {
        [SerializeField] private GameObject lightFragmentPrefab;
        [SerializeField] private CircleCollider2D maxInfluenceCollider;
        [SerializeField] private CircleCollider2D minInfluenceCollider;
        [SerializeField] private float shootDuration = 1.0f;
        [SerializeField] private float shootInterval = 5;
        private float lastShotTime;

        [Header("Cooldown")]
        public bool isCoolingDown;
        public float spawnLightFragCooldown = 3;
        [HideInInspector] public float cooldownStartTime;

        public override void Awake()
        {
            base.Awake();
            shootInterval = Util.upgradeSettings.mitoShootInterval_float.GetFloat();
            maxHealth = Util.upgradeSettings.mitoTowerMaxHealth;
            currHealth = maxHealth;
            healthBar.SetHealth(currHealth);
        }

        private GameObject CreateFragment()
        {
            GameObject fragment = Instantiate(lightFragmentPrefab);
            return fragment;
        }

        public override void OnUpdate()
        {
            if (isCoolingDown)
            {
                if (Time.time > cooldownStartTime + spawnLightFragCooldown)
                {
                    isCoolingDown = false;
                    EventManager.UI.onSpawnLightDropCooldownComplete?.Invoke(this);
                }
            }
        }

        [Button("Shoot Fragment")]
        public void ShootFragment()
        {
            if (Time.time < lastShotTime + shootInterval)
                return;

            var fragment = CreateFragment();
            Vector3 startPos = transform.position;
            Vector3 endPos = GetPointWithinInfluence();
            Vector3 controlPoint = startPos + (endPos - startPos) * 0.5f + Vector3.up;

            var seq = LeanTween.sequence();

            seq.append(
                LeanTween.value(gameObject, 0, 1, shootDuration)
                .setOnUpdate((float val) =>
                {
                    Vector2 targetPos = Util.Bezier2(startPos, controlPoint, endPos, val);
                    fragment.transform.position = targetPos;
                })
                .setEaseInSine()
            );

            seq.append(LeanTween.moveY(fragment, endPos.y + 0.06f, 0.1f));
            seq.append(LeanTween.moveY(fragment, endPos.y, 0.1f));
            lastShotTime = Time.time;
            EventManager.Structures.onLightDropped?.Invoke();
        }

        public Vector2 GetPointWithinInfluence()
        {
            return Util.GetPointWithinInfluence(
                    minInfluenceCollider.transform.position,
                    minInfluenceCollider.radius,
                    maxInfluenceCollider.radius
                );
        }

        private void OnTapLightDropButton(MitoTower tower)
        {
            if (tower != this)
                return;

            isCoolingDown = true;
            cooldownStartTime = Time.time;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            EventManager.UI.onTapLightDropButton += OnTapLightDropButton;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            EventManager.UI.onTapLightDropButton -= OnTapLightDropButton;
        }
    }
}