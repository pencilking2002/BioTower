using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using UnityEngine.UI;

namespace BioTower.UI
{
    public enum ScreenEdge { LEFT, RIGHT, TOP, BOTTOM }
    public class WaveWarningIcon : MonoBehaviour
    {
        [SerializeField] private RectTransform panel;


        [Header("Enemies")]
        [SerializeField] private Image basicEnemy;
        [SerializeField] private Image midEnemy;
        [SerializeField] private Image advEnemy;


        [Header("Arrows")]
        [SerializeField] private Image leftArrow;
        [SerializeField] private Image rightArrow;
        [SerializeField] private Image topArrow;
        [SerializeField] private Image bottomArrow;

        private RectTransform rt;
        private Dictionary<UnitType, Image> enemyUnitDict;
        private Dictionary<ScreenEdge, Vector2> screenEdgeDict;
        private Dictionary<ScreenEdge, Image> arrowDict;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            enemyUnitDict = new Dictionary<UnitType, Image>() {
                { UnitType.BASIC_ENEMY, basicEnemy},
                { UnitType.MID_ENEMY, midEnemy },
                { UnitType.ADVANCED_ENEMY, advEnemy }
            };

            screenEdgeDict = new Dictionary<ScreenEdge, Vector2> {
                { ScreenEdge.LEFT, new Vector2(0, Screen.height / 2)},
                { ScreenEdge.RIGHT, new Vector2(Screen.width, Screen.height / 2)},
                { ScreenEdge.BOTTOM, new Vector2(Screen.width / 2, 0)},
                { ScreenEdge.TOP, new Vector2(Screen.width / 2, Screen.height)}
            };

            arrowDict = new Dictionary<ScreenEdge, Image> {
                { ScreenEdge.LEFT, leftArrow},
                { ScreenEdge.RIGHT, rightArrow},
                { ScreenEdge.TOP, topArrow},
                { ScreenEdge.BOTTOM, bottomArrow},
            };
        }

        private Vector2 GetScreenEdge(ScreenEdge edge)
        {
            return screenEdgeDict[edge];
        }

        private Vector2 GetClosestScreenEdge(Vector2 screenPos)
        {
            float closestDistance = Mathf.Infinity;
            Vector2 closestEdge = Vector2.zero;
            foreach (KeyValuePair<ScreenEdge, Vector2> item in screenEdgeDict)
            {
                float distance = Vector2.Distance(screenPos, item.Value);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEdge = item.Value;
                }
            }
            return closestEdge;
        }

        private Vector2 ClampScreenPoint(Vector2 screenPoint)
        {
            float inwardOffset = 80;

            if (screenPoint.x < screenEdgeDict[ScreenEdge.LEFT].x)
            {
                var leftX = screenEdgeDict[ScreenEdge.LEFT].x;
                screenPoint.x = leftX + inwardOffset;
                DisplayArrow(ScreenEdge.LEFT);
            }
            else if (screenPoint.x > screenEdgeDict[ScreenEdge.RIGHT].x)
            {
                var rightX = screenEdgeDict[ScreenEdge.RIGHT].x;
                screenPoint.x = rightX - inwardOffset;
                DisplayArrow(ScreenEdge.RIGHT);
            }

            if (screenPoint.y > screenEdgeDict[ScreenEdge.TOP].y)
            {
                var topY = screenEdgeDict[ScreenEdge.TOP].y;
                screenPoint.y = topY - inwardOffset;
                DisplayArrow(ScreenEdge.TOP);
            }
            else if (screenPoint.y < screenEdgeDict[ScreenEdge.BOTTOM].y)
            {
                var bottomY = screenEdgeDict[ScreenEdge.BOTTOM].y;
                screenPoint.y = bottomY + inwardOffset;
                DisplayArrow(ScreenEdge.BOTTOM);
            }
            return screenPoint;
        }

        private void SetIconPosition()
        {
            var spawnIndex = Util.waveManager.currWave.waypointIndex;
            var spawnPoint = GameManager.Instance.GetWaypointManager().GetSpawnPoint(spawnIndex);
            //var spawnPointScreenPos = Camera.main.WorldToScreenPoint(spawnPoint.transform.position);
            // var screenEdge = GetClosestScreenEdge(spawnPointScreenPos);

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, spawnPoint.transform.position);
            screenPoint = ClampScreenPoint(screenPoint);

            //screenPoint = screenPoint + difference + difference.normalized * 100f;
            rt.anchoredPosition = screenPoint - Util.tutCanvas.GetComponent<RectTransform>().sizeDelta / 2f;
        }

        private void DisplayIcon(UnitType unitType)
        {
            panel.gameObject.SetActive(true);
            foreach (KeyValuePair<UnitType, Image> item in enemyUnitDict)
                item.Value.enabled = item.Key == unitType;
        }

        private void DisplayArrow(ScreenEdge edge)
        {
            foreach (KeyValuePair<ScreenEdge, Image> item in arrowDict)
                item.Value.enabled = item.Key == edge;
        }

        private void HideIcon()
        {
            panel.gameObject.SetActive(false);
        }

        private void OnWaveStateInit(WaveMode waveMode)
        {
            if (waveMode == WaveMode.DELAY)
            {
                var enemyType = Util.waveManager.currWave.enemyType;
                DisplayIcon(enemyType);
                SetIconPosition();
            }
        }

        private void OnEnable()
        {
            EventManager.Wave.onWaveStateInit += OnWaveStateInit;
        }

        private void OnDisable()
        {
            EventManager.Wave.onWaveStateInit -= OnWaveStateInit;
        }
    }
}