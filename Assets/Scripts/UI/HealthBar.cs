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
        private int health;
        private RectTransform canvasRT;
        private void Awake()
        {
            canvasRT = GetComponent<RectTransform>();
            Init(10);
            //Debug.Break();
        }

        public void Init(int health)
        {
            this.health = health;
            // float sizeX = (canvasRT.sizeDelta.x * (health - 1)) / health;
            // float offsetX = sizeX;
            // for (int i = 0; i < health; i++)
            // {
            //     var bar = Instantiate(barPrefab);
            //     bar.name = $"{bar.name}_{i}";
            //     bar.transform.SetParent(healthPanel);
            //     var rt = (RectTransform)bar.transform;

            //     rt.sizeDelta = new Vector2(-sizeX, 0);
            //     rt.anchoredPosition = new Vector2(-offsetX, 0);
            //     offsetX -= sizeX;

            //     // bar.transform.localPosition = Vector3.zero;
            // }
            Vector2 scale = new Vector2(1.0f / health, 1);
            float halfCanvas = canvasRT.sizeDelta.x / 2;
            float initialPosition = -halfCanvas + halfCanvas * scale.x;
            float offsetX = initialPosition;
            for (int i = 0; i < health; i++)
            {
                var go = Instantiate(barPrefab);
                go.name = $"{go.name}_{i}";
                go.transform.SetParent(healthPanel);
                var rt = (RectTransform)go.transform;
                go.transform.localPosition = new Vector2(offsetX, 0);
                //go.TryGetComponent.localPosition += new Vector2(0,0);
                rt.sizeDelta = Vector3.zero;
                rt.localScale = scale;
                //offsetX -= initialPosition * 2;
            }
        }

        // private GameObject CreateBar()
        // {
        //     var bar = Instantiate(barPrefab);
        //     bar.transform.SetParent(healthPanel);
        //     bar.transform.SetAsFirstSibling();
        //     return bar;
        // }

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
