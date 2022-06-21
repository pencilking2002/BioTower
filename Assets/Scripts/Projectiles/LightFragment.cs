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
        private Vector3 initScale = Vector3.one;


        private void Awake()
        {
            pooledObject = GetComponent<PooledObject>();

        }

        public void OnCreate()
        {
            transform.localScale = initScale;
        }

        public void DestroyObject(bool isInstant = false)
        {
            if (isInstant)
            {
                LeanTween.cancel(gameObject);
                transform.localScale = Vector3.zero;
                hasBeenPickedUp = false;
                pooledObject.SendToPool();
                return;
            }

            if (hasBeenPickedUp)
                return;

            transform.localScale = initScale;
            hasBeenPickedUp = true;
            GameManager.Instance.CreateLightExplosion(transform.position);
            Util.objectShake.Shake(GameManager.Instance.cam.gameObject, 0.2f, 0.02f);

            LeanTween.cancel(gameObject);
            var seq = LeanTween.sequence();

            seq.append(LeanTween.scale(gameObject, initScale * 3f, 0.1f));

            seq.append(LeanTween.scale(gameObject, Vector3.zero, 0.1f));

            seq.append(gameObject, () => { SendTrail(0.15f); });

            seq.append(0.15f);

            seq.append(gameObject, () =>
            {
                transform.localScale = Vector3.zero;
                hasBeenPickedUp = false;
                pooledObject.SendToPool();
                EventManager.Game.onLightFragmentTapped?.Invoke();
            });

            EventManager.Structures.onLightPickedUp?.Invoke();
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

        private void OnGameOver(bool isWin, float delay)
        {
            //if (transform.parent == null)
            DestroyObject(true);
        }

        private void OnStartLevel(LevelType levelType)
        {
            DestroyObject(true);
        }

        private void OnEnable()
        {
            EventManager.Game.onGameOver += OnGameOver;
            EventManager.Game.onLevelStart += OnStartLevel;
        }

        private void OnDisable()
        {
            EventManager.Game.onGameOver -= OnGameOver;
            EventManager.Game.onLevelStart -= OnStartLevel;
        }
    }
}