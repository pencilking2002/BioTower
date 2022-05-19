using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

namespace BioTower.UI
{
    public class ProgressCanvas : MonoBehaviour
    {
        [SerializeField] private Disc progressDisc;
        private Vector3 initScale;

        private void Awake()
        {
            initScale = progressDisc.transform.localScale;
            gameObject.SetActive(false);
        }

        public void StartProgress(float duration)
        {
            gameObject.SetActive(true);
            var seq = LeanTween.sequence();
            progressDisc.transform.localScale = Vector3.zero;
            seq.append(LeanTween.scale(progressDisc.gameObject, initScale, 0.25f));

            seq.append(LeanTween.value(progressDisc.gameObject, 360, 0, duration)
            .setOnUpdate((float val) =>
            {
                progressDisc.AngRadiansEnd = val * Mathf.Deg2Rad;
            }));
            seq.append(gameObject, () =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}
