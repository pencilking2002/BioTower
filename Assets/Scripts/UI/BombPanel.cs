using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BioTower.Units;
using TMPro;
using UnityEngine.U2D;

namespace BioTower
{
    public class BombPanel : MonoBehaviour
    {
        public GameObject bombPrefab;
        [SerializeField] private Image progressImage;
        [SerializeField] private Button bombButton;
        [SerializeField] private TextMeshProUGUI bombText;
        [SerializeField] private bool canPressBombButton;

        private Vector3 onscreenPos;
        private Vector3 offscreenPos;

        private SpriteShapeRenderer[] roads;
        private Color initRoadColor;

        private void Awake()
        {
            onscreenPos = bombText.transform.position;
            bombText.transform.position += new Vector3(0, 200, 0);
            offscreenPos = bombText.transform.position;

            var roadGO = GameObject.FindGameObjectWithTag(Constants.roads);
            roads = roadGO.GetComponentsInChildren<SpriteShapeRenderer>();
            initRoadColor = roads[0].color;
        }

        public void OnPressBombButton()
        {
            if (!canPressBombButton)
                return;

            if (Util.placementManager.IsNoneState())
            {
                Util.placementManager.SetBombPlacingState();
                canPressBombButton = false;
                progressImage.fillAmount = 0;
                SlideInBombText();

                foreach (var road in roads)
                {
                    LeanTween.value(gameObject, initRoadColor, Color.white, 0.25f)
                    .setOnUpdate((Color col) =>
                    {
                        road.color = col;
                    })
                    .setLoopPingPong(-1);
                }
            }
        }

        private void Update()
        {
            bombButton.interactable = canPressBombButton;
        }

        private void SlideInBombText()
        {
            bombText.transform.LeanMove(onscreenPos, 0.25f);
        }

        private void SlideOutBombText()
        {
            bombText.transform.LeanMove(offscreenPos, 0.25f);
        }

        public void IncreaseBombEnergy()
        {
            var progress = progressImage.fillAmount;
            progress += Util.gameSettings.enemyBombEnergy;
            progress = Mathf.Clamp01(progress);
            progressImage.fillAmount = progress;

            if (Mathf.Approximately(progress, 1.0f))
            {
                canPressBombButton = true;
            }
        }

        private void OnUnitDestroyed(Unit unit)
        {
            if (!unit.IsEnemy())
                return;

            IncreaseBombEnergy();
        }

        private void OnLevelStart(LevelType levelType)
        {
            bombText.transform.position = offscreenPos;
            canPressBombButton = false;
            progressImage.fillAmount = 0;
        }

        private void OnPlaceBomb()
        {
            SlideOutBombText();
            LeanTween.cancel(gameObject);
            foreach (var road in roads)
                road.color = initRoadColor;
        }

        private void OnEnable()
        {
            EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
            EventManager.Game.onLevelStart += OnLevelStart;
            EventManager.Structures.onPlaceBomb += OnPlaceBomb;
        }

        private void OnDisable()
        {
            EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
            EventManager.Game.onLevelStart -= OnLevelStart;
            EventManager.Structures.onPlaceBomb -= OnPlaceBomb;
        }
    }

}
