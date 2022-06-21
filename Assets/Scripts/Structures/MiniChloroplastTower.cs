using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace BioTower.Structures
{
    [SelectionBase]
    public class MiniChloroplastTower : Structure
    {
        [SerializeField] private CircleCollider2D maxInfluenceCollider;
        [SerializeField] private CircleCollider2D minInfluenceCollider;
        [SerializeField] private float shootDuration = 1.0f;
        [SerializeField] private float shootInterval = 3;
        private float lastShotTime;
        private Vector3 initScale;

        public override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            base.Init(null);
            lastShotTime = Time.time;
            initScale = sr.transform.localScale;
            sr.transform.localScale = Vector3.zero;
            LeanTween.cancel(sr.gameObject);
            var seq = LeanTween.sequence();
            seq.append(UnityEngine.Random.Range(0.0f, 1.0f));
            seq.append(LeanTween.scale(sr.gameObject, initScale * 2, 0.25f));
            seq.append(LeanTween.scale(sr.gameObject, initScale, 0.25f).setEaseOutBack());
            seq.append(() =>
            {
                lastShotTime = Time.time - shootInterval + (UnityEngine.Random.Range(0.5f, 1.0f));
            });
            EventManager.Structures.onStructureActivated?.Invoke(this);
        }


        public override void OnUpdate()
        {
            if (Time.time > lastShotTime + shootInterval)
            {
                ShootFragment(1);
                lastShotTime = Time.time + UnityEngine.Random.Range(0.0f, 1.0f);
            }
        }

        private GameObject CreateFragment()
        {
            PooledObject obj = Util.poolManager.GetPooledObject(PoolObjectType.LIGHT_FRAGMENT);
            return obj.gameObject;
        }

        public void ShootFragment(int numFragments, bool avoidFragmentCollider = true)
        {
            DoSquishyAnimation();


            for (int i = 0; i < numFragments; i++)
            {
                Vector3 startPos = transform.position;
                Vector3 endPos = GetPointWithinInfluence(avoidFragmentCollider);
                Vector3 controlPoint = startPos + (endPos - startPos) * 0.5f + Vector3.up;

                var fragment = CreateFragment();
                fragment.transform.position = startPos;
                var seq = LeanTween.sequence();
                seq.append(0.1f * i);

                seq.append(() =>
                {
                    EventManager.Structures.onLightDropped?.Invoke();
                });

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

                seq.append(() =>
                {
                    if (Util.tutCanvas.hasTutorials && Util.tutCanvas.currTutorial.highlightedItem == HighlightedItem.MINI_CHLORO)
                    {
                        Util.poolManager.SpawnItemHighlight(fragment.transform.position, new Vector2(0, 120));
                    }
                });

            }
        }

        private void DoSquishyAnimation()
        {
            var wideScale = initScale;
            wideScale.x *= 1.5f;
            var tallScale = initScale;
            tallScale.y *= 2f;

            sr.transform.localScale = initScale;
            var seq = LeanTween.sequence();
            seq.append(LeanTween.scale(sr.gameObject, wideScale, 0.2f));
            seq.append(LeanTween.scale(sr.gameObject, tallScale, 0.1f));
            seq.append(LeanTween.scale(sr.gameObject, initScale, 0.2f));
            seq.append(gameObject, () => { sr.transform.localScale = initScale; });
        }

        public Vector2 GetPointWithinInfluence(bool avoidFragmentCollider)
        {
            Vector2 randomPoint = GetRandomPoint();
            return randomPoint;
        }

        private Vector2 GetRandomPoint()
        {
            return Util.GetPointWithinInfluence(
                minInfluenceCollider.transform.position,
                minInfluenceCollider.radius,
                maxInfluenceCollider.radius
            );
        }
    }
}