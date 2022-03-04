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

        public void Init(int health)
        {
            this.health = health;
            for (int i = 0; i < healthPanel.childCount; i++)
            {
                healthPanel.GetChild(i).gameObject.SetActive(i < health - 1);

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
