using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using Sirenix.OdinInspector;
using BioTower.UI;

namespace BioTower
{

    [SelectionBase]
    public class StructureSocket : MonoBehaviour
    {
        [SerializeField] private ProgressCanvas progressCanvas;
        [SerializeField] private ParticleSystem buildingParticles;
        [ReadOnly][SerializeField] private bool isBuildingStructure;
        [ReadOnly][SerializeField] private bool hasStructure;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Color glowColor;
        [SerializeField] private float glowAnimDuration = 0.5f;
        private Color defaultColor;
        private Vector3 initScale;
        private bool isAnimatingIn = true;

        private void Start()
        {
            var delay = UnityEngine.Random.Range(0.0f, 1.0f);
            var duration = UnityEngine.Random.Range(0.5f, 1.0f);
            var scale = sr.transform.localScale;
            var additionalScale = UnityEngine.Random.Range(0.0f, 0.2f);

            var color = sr.color;
            color.r += UnityEngine.Random.Range(0.0f, 0.1f);
            color.g += UnityEngine.Random.Range(0.0f, 0.1f);
            color.b += UnityEngine.Random.Range(0.0f, 0.1f);

            sr.color = color;
            sr.transform.localScale = Vector3.zero;
            defaultColor = sr.color;

            // Do a scaling animation and record the scale
            var seq = LeanTween.sequence();
            seq.append(1 + delay);
            seq.append(() => { EventManager.Structures.onSocketPop?.Invoke(this); });
            seq.append(LeanTween.scale(sr.gameObject, scale * (1 + additionalScale), duration).setEaseOutElastic());
            seq.append(sr.gameObject, () =>
            {
                initScale = sr.transform.localScale;
                isAnimatingIn = false;
            });
            EventManager.Structures.onSocketStart?.Invoke(this);
        }

        public void StartProgress(float duration)
        {
            progressCanvas.StartProgress(duration);
        }

        public void OnTap()
        {
            if (HasNone() && !isAnimatingIn)
            {
                //Debug.Log("Tap. HasNone: " + HasNone());
                Jiggle();
                EventManager.Structures.onTapFreeStructureSocket?.Invoke(this);
            }
        }

        public void Jiggle()
        {
            LeanTween.cancel(sr.gameObject);
            sr.transform.localScale = initScale;
            float randScaleX = UnityEngine.Random.Range(1.4f, 1.6f);
            float randScaleY = UnityEngine.Random.Range(1.2f, 1.4f);
            var newScale = initScale;
            newScale.x *= randScaleX;
            newScale.y *= randScaleY;

            var seq = LeanTween.sequence();
            seq.append(LeanTween.scale(sr.gameObject, newScale, 0.1f));
            seq.append(LeanTween.scale(sr.gameObject, initScale, 0.5f).setEaseOutElastic());
        }



        private void OnStartPlacementState(StructureType structureType)
        {
            if (hasStructure)
                return;

            var currColor = sr.color;
            LeanTween.cancel(gameObject);
            LeanTween.value(gameObject, currColor, glowColor, glowAnimDuration).setOnUpdate((Color col) =>
            {
                sr.color = col;
            }).setLoopPingPong(-1);

            LeanTween.delayedCall(0.1f, () =>
            {
                var item = Util.poolManager.SpawnItemHighlight(this.transform.position, new Vector2(0, 100));
            });
        }

        public bool HasStructure()
        {
            return hasStructure;
        }

        public bool HasNone() { return !this.hasStructure; }

        public void SetHasNone()
        {
            this.hasStructure = false;
            this.isBuildingStructure = false;
        }

        public void SetHasStructure()
        {
            LeanTween.cancel(sr.gameObject);
            this.hasStructure = true;
            this.isBuildingStructure = false;
            buildingParticles.Stop();
        }

        private void OnSetNonePlacementState()
        {
            LeanTween.cancel(gameObject);
            sr.color = defaultColor;
            Util.poolManager.DespawnAllitemHighlights();
        }

        public void SetIsBuildingStructure()
        {
            //LeanTween.cancel(sr.gameObject);
            isBuildingStructure = true;
            hasStructure = false;
            buildingParticles.Play();
            Util.poolManager.DespawnAllitemHighlights();
            LeanTween.value(sr.gameObject, defaultColor, glowColor, 0.25f).setLoopPingPong(-1);
        }

        public bool IsBuildingStructure() { return this.isBuildingStructure; }

        private void OnHighlightItem(HighlightedItem item)
        {
            // if (item == HighlightedItem.SOCKET)
            // {
            //     Util.poolManager.SpawnItemHighlight(this.transform.position, new Vector2(0,100));
            // }
        }

        private void OnEnable()
        {
            EventManager.Structures.onStartPlacementState += OnStartPlacementState;
            EventManager.Structures.onSetNonePlacementState += OnSetNonePlacementState;
            EventManager.Tutorials.onHighlightItem += OnHighlightItem;
        }

        private void OnDisable()
        {
            EventManager.Structures.onStartPlacementState -= OnStartPlacementState;
            EventManager.Structures.onSetNonePlacementState -= OnSetNonePlacementState;
            EventManager.Tutorials.onHighlightItem -= OnHighlightItem;
        }
    }
}