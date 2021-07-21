using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace BioTower
{
public class ObjectShake : MonoBehaviour
{
    [ReadOnly][SerializeField] private bool isShaking;
    public void Shake(GameObject go, float duration, float amount)
    {
        if (isShaking)
            return;
        
        isShaking = true;
        Vector3 originalPos = go.transform.localPosition;

        var seq = LeanTween.sequence();

        seq.append(() => {
            LeanTween.value(gameObject, 0, 1, duration).setOnUpdate((float val) => {
                go.transform.localPosition = originalPos + Random.insideUnitSphere * amount;
            });
        });

        seq.append(() => {
            go.transform.localPosition = originalPos;
            isShaking = false;
        });
    }
}
}