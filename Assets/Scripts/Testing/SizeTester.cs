using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    public class SizeTester : MonoBehaviour
    {
        public RectTransform containerRT;

        public RectTransform fullheightRT;

        private void Update()
        {
            var topY = GetHighestPoint();
            var bottomY = GetLowestPoint();

            var sizeDelta = fullheightRT.sizeDelta;
            sizeDelta.y = topY - bottomY;
            fullheightRT.sizeDelta = sizeDelta;
            fullheightRT.position = new Vector2(fullheightRT.position.x, bottomY + sizeDelta.y / 2);

        }

        private float GetHighestPoint()
        {
            float highest = -100000;
            for (int i = 0; i < containerRT.childCount; i++)
            {
                RectTransform child = (RectTransform)containerRT.GetChild(i);
                if (child.position.y > highest)
                {
                    highest = child.position.y + child.sizeDelta.y / 2;
                }
            }
            return highest;
        }

        private float GetLowestPoint()
        {
            float lowest = Mathf.Infinity;
            for (int i = 0; i < containerRT.childCount; i++)
            {
                RectTransform child = (RectTransform)containerRT.GetChild(i);
                if (child.position.y < lowest)
                {
                    lowest = child.position.y - child.sizeDelta.y / 2;
                }
            }
            return lowest;
        }
    }
}
