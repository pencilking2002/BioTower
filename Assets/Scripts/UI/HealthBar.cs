using UnityEngine;

namespace BioTower.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject barPrefab;
        [SerializeField] private Vector2 padding = new Vector2(0.1f, 0.1f);
        private int health;
        private RectTransform canvasRT;
        private void Awake()
        {
            canvasRT = GetComponent<RectTransform>();
        }

        public void Init(int health)
        {
            this.health = health;
            Vector2 scale = new Vector2(1.0f / health, 1);
            float halfCanvas = canvasRT.sizeDelta.x / 2;

            float initialPosition = (-halfCanvas + halfCanvas * scale.x);
            float offsetX = initialPosition;
            float barSize = (halfCanvas * scale.x) * 2;

            // Process padding to work with Unity's UI coord system
            padding = new Vector2(padding.x / 2, padding.y);
            padding = -padding;

            for (int i = 0; i < health; i++)
            {
                var go = Instantiate(barPrefab);
                go.name = $"{go.name}_{i}";
                go.transform.SetParent(transform);
                var bar = go.GetComponent<Bar>();
                //var rt = (RectTransform)go.transform;
                go.transform.localPosition = new Vector2(offsetX, 0);
                bar.rt.sizeDelta = Vector3.zero;
                //var child = (RectTransform)rt.GetChild(0);
                bar.innerRT.sizeDelta = padding;
                //child.sizeDelta = Vector2.zero;
                bar.rt.localScale = scale;
                offsetX += barSize;
            }
        }

        public void SetHealth(int health)
        {
            this.health = health;
            if (this.health <= 0)
                return;

            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(i < health - 1);

        }
    }
}
