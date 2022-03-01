using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    public class LightFragment : MonoBehaviour
    {
        public bool hasBeenPickedUp;
        private PooledObject pooledObject;

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

            seq.append(gameObject, () =>
            {
                transform.localScale = scale;
                pooledObject.SendToPool();
                hasBeenPickedUp = false;
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