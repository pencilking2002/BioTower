using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BioTower.Units;

namespace BioTower
{
    public class BombPanel : MonoBehaviour
    {
        public GameObject bombPrefab;
        [SerializeField] private Image progressImage;
        [SerializeField] private Button bombButton;
        [SerializeField] private bool canPressBombButton;

        public void OnPressBombButton()
        {
            if (!canPressBombButton)
                return;

            if (Util.placementManager.IsNoneState())
                Util.placementManager.SetBombPlacingState();

            canPressBombButton = false;
            progressImage.fillAmount = 0;
        }

        private void Update()
        {
            bombButton.interactable = canPressBombButton;
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
            canPressBombButton = false;
            progressImage.fillAmount = 0;
        }

        private void OnEnable()
        {
            EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
            EventManager.Game.onLevelStart += OnLevelStart;
        }

        private void OnDisable()
        {
            EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
            EventManager.Game.onLevelStart -= OnLevelStart;
        }
    }

}
