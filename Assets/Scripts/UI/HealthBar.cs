using UnityEngine;
using UnityEngine.UI;

namespace BioTower.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject barPrefab;
        [HideInInspector] private Image healthBarBG;
        [SerializeField] private Vector2 padding = new Vector2(0.1f, 0.1f);
        private int health;
        [SerializeField] private Color barColor;
        [SerializeField] private Color deactivatedBarColor;
        [SerializeField] private Color bgColor;
        [SerializeField] private Bar[] bars;
        private RectTransform canvasRT;
        private void Awake()
        {
            canvasRT = GetComponent<RectTransform>();
            healthBarBG = transform.GetChild(0).GetComponent<Image>();
        }

        public void Init(int health)
        {
            bars = new Bar[health];
            this.health = health;

            Vector2 scale = new Vector2(1.0f / health, 1);
            float halfCanvas = canvasRT.sizeDelta.x / 2;

            float initialPosition = (-halfCanvas + halfCanvas * scale.x);
            float offsetX = initialPosition;
            float barSize = (halfCanvas * scale.x) * 2;

            // Process padding to work with Unity's UI coord system
            padding = new Vector2(padding.x / 2, padding.y);
            padding = -padding;

            healthBarBG.color = bgColor;

            for (int i = 0; i < health; i++)
            {
                var go = Instantiate(barPrefab);
                go.name = $"{go.name}_{i}";
                go.transform.SetParent(transform);
                go.transform.localPosition = new Vector2(offsetX, 0);

                var bar = go.GetComponent<Bar>();
                bar.rt.sizeDelta = Vector3.zero;
                bar.innerRT.sizeDelta = padding;
                bar.rt.localScale = scale;
                bar.SetColor(barColor);
                bars[i] = bar;

                offsetX += barSize;
            }
        }

        public void SetHealth(int health)
        {
            this.health = health;
            if (this.health <= 0)
                return;

            for (int i = 0; i < bars.Length; i++)
            {
                var bar = bars[i];
                if (bar)
                {
                    Color color = i < health ? barColor : deactivatedBarColor;
                    bar.SetColor(color);
                }
            }

        }
    }
}
