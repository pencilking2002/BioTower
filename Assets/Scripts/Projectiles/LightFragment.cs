using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    public class LightFragment : MonoBehaviour
    {
        public bool hasBeenPickedUp;
        private PooledObject pooledObject;
        [SerializeField] private GameObject trailPrefab;

        private void Awake()
        {
            pooledObject = GetComponent<PooledObject>();

        }

        public void DestroyObject()
        {
            if (hasBeenPickedUp)
                return;

            GameManager.Instance.CreateLightExplosion(transform.position);
            hasBeenPickedUp = true;
            EventManager.Structures.onLightPickedUp?.Invoke();

            var scale = transform.localScale;

            Util.objectShake.Shake(GameManager.Instance.cam.gameObject, 0.2f, 0.02f);

            var seq = LeanTween.sequence();

            seq.append(LeanTween.scale(gameObject, scale * 3f, 0.1f));

            seq.append(LeanTween.scale(gameObject, Vector3.zero, 0.1f));

            seq.append(() => { SendTrail(0.15f); });

            seq.append(0.15f);

            seq.append(gameObject, () =>
            {
                EventManager.Game.onLightFragmentTapped?.Invoke();
                transform.localScale = scale;
                pooledObject.SendToPool();
                hasBeenPickedUp = false;
            });
        }

        private void SendTrail(float duration)
        {
            var go = Instantiate(trailPrefab);
            go.transform.position = transform.position;
            var currencyRT =
                Util.bootController.gameplayUI.currencyContainer.GetComponent<RectTransform>();

            var targetPos = Util.GetUIWorldPos(currencyRT);
            targetPos.z = transform.position.z;

            LTSpline ltSpline = new LTSpline(
                    new Vector3[] {
                        new Vector3(UnityEngine.Random.Range(-10,10), UnityEngine.Random.Range(-10,10), 0f),
                        transform.position,
                        targetPos,
                        targetPos
                    });

            // for (int i = 0; i < 10; i++)
            // {
            //     if (i + 1 < 10)
            //     {
            //         var point1 = ltSpline.point(i / 10.0f);
            //         var point2 = ltSpline.point((i + 1) / 10.0f);
            //         Debug.DrawLine(point1, point2, Color.red);
            //     }
            // }

            // Debug.Break();

            LeanTween.moveSpline(go, ltSpline, duration).setOnComplete(() =>
            {
                Destroy(go);
            });
        }

        private void OnGameOver(bool isWin)
        {
            if (transform.parent == null)
                DestroyObject();
        }

        private void OnEnable()
        {
            EventManager.Game.onGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            EventManager.Game.onGameOver -= OnGameOver;
        }
    }
}