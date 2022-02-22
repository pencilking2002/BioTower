using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace BioTower
{
    public class ObjectShake : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private bool isShaking;
        public void Shake(GameObject go, float duration, float amount)
        {
            if (isShaking)
                return;

            isShaking = true;
            Vector3 originalPos = go.transform.localPosition;

            var seq = LeanTween.sequence();

            seq.append(() =>
            {
                LeanTween.value(gameObject, 0, 1, duration).setOnUpdate((float val) =>
                {
                    go.transform.localPosition = originalPos + Random.insideUnitSphere * amount;
                }).setIgnoreTimeScale(true);
            });

            seq.append(() =>
            {
                go.transform.localPosition = originalPos;
                isShaking = false;
            });
        }

        public void Shake(float duration, float amount)
        {
            Shake(gameObject, duration, amount);
        }

        public void ShakeHorizontal(GameObject go, float duration, float amount)
        {
            if (isShaking)
                return;

            isShaking = true;
            Vector3 originalPos = go.transform.localPosition;

            var seq = LeanTween.sequence();

            seq.append(() =>
            {
                LeanTween.value(gameObject, 0, 1, duration).setOnUpdate((float val) =>
                {
                    var newPos = originalPos + Random.insideUnitSphere * amount;
                    newPos = originalPos + Random.insideUnitSphere * amount;
                    newPos.y = originalPos.y;
                    newPos.z = originalPos.z;
                    go.transform.localPosition = newPos;
                });
            });

            seq.append(() =>
            {
                go.transform.localPosition = originalPos;
                isShaking = false;
            });
        }

        public void ShakeHorizontal(float duration, float amount)
        {
            ShakeHorizontal(gameObject, duration, amount);
        }
    }
}