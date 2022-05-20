using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BioTower
{
    public class UnitCounterCanvas : MonoBehaviour
    {
        [SerializeField] private Image[] unitCircles;

        private void SetCircles(int numActive, int total)
        {
            for (int i = 0; i < unitCircles.Length; i++)
            {
                Image circle = unitCircles[i];
                bool isLessThanTotal = i < total;
                circle.gameObject.SetActive(isLessThanTotal);
                circle.color = i < numActive ? Color.yellow : Color.black;
            }
        }
        public void OnUpdate(int numUnits)
        {
            SetCircles(numUnits, 3);
        }
    }
}
