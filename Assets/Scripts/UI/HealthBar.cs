using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace BioTower
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject barPrefab;
        [SerializeField] private RectTransform healthPanel;
        [SerializeField] private Vector2 padding = new Vector2(0.5f, 0);
        private int health;
        private RectTransform canvasRT;
        private void Awake()
        {
            canvasRT = GetComponent<RectTransform>();
            Init(5);
            //Debug.Break();
        }

        public void Init(int health)
        {
            this.health = health;
            Vector2 scale = new Vector2(1.0f / health, 1);
            float halfCanvas = canvasRT.sizeDelta.x / 2;

            float initialPosition = (-halfCanvas + halfCanvas * scale.x);
            float offsetX = initialPosition;
            float barSize = (halfCanvas * scale.x) * 2;
            for (int i = 0; i < health; i++)
            {
                var go = Instantiate(barPrefab);
                go.name = $"{go.name}_{i}";
                go.transform.SetParent(healthPanel);
                var rt = (RectTransform)go.transform;
                go.transform.localPosition = new Vector2(offsetX, 0);
                rt.sizeDelta = Vector3.zero;
                rt.localScale = scale;
                offsetX += barSize;
            }
        }

        public void SetHealth(int health)
        {
            this.health = health;
            if (this.health <= 0)
                return;

            for (int i = 0; i < healthPanel.childCount; i++)
                healthPanel.GetChild(i).gameObject.SetActive(i < health - 1);

        }
    }
}
