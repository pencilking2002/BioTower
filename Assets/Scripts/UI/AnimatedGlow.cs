using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.UI
{
    public class AnimatedGlow : MonoBehaviour
    {
        [SerializeField] private Vector2 scaleStrength;
        [SerializeField] private Vector2 scaleDuration;
        public void StartGlowing()
        {
            LeanTween.scaleX(gameObject, scaleStrength.x, scaleDuration.x).setLoopPingPong(-1).setIgnoreTimeScale(true);
            LeanTween.scaleY(gameObject, scaleStrength.y, scaleDuration.y).setLoopPingPong(-1).setIgnoreTimeScale(true);

            //Debug.Log("Start glowing");
        }

        public void StopGlowing()
        {
            LeanTween.cancel(gameObject);
            //Debug.Log("Stop glowing");
        }
    }
}
