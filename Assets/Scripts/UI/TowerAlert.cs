using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using BioTower.Structures;

namespace BioTower
{
    public class TowerAlert : MonoBehaviour
    {
        //[SerializeField] private Animator anim;
        //private bool isAnimating;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Structure tower;
        [SerializeField] Color weakColor;


        public void OnUpdate()
        {
            if (tower.structureType == StructureType.ABA_TOWER)
            {
                // if (TutorialCanvas.tutorialInProgress)
                // {
                //     if (Util.tutCanvas.tutState != TutState.WAITING_BUTTON_TAP &&
                //         Util.tutCanvas.tutState != TutState.LETTER_REVEAL)
                //     {
                //         SetWeakColor();
                //         //Debug.Log("set weak color");
                //     }
                //     else
                //     {
                //         SetDefaultColor();
                //     }
                // }
                if (tower.GetNumUnits() == 0)
                    SetWeakColor();
                else if (!LeanTween.isTweening(sr.gameObject))
                    SetDefaultColor();
            }
        }
        public void SetWeakColor()
        {
            sr.color = weakColor;
        }

        public void SetDefaultColor()
        {
            sr.color = Color.white;
        }

        private void OnUnitDestroyed(Unit unit)
        {
            if (unit.tower == tower)
            {
                if (tower.GetNumUnits() <= 1)
                    SetWeakColor();
            }
        }

        private void OnUnitSpawned(Unit unit)
        {
            if (unit.tower == tower)
            {
                SetDefaultColor();
            }
        }

        // private void OnEnable()
        // {
        //     EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
        //     EventManager.Units.onUnitSpawned += OnUnitSpawned;
        // }

        // private void OnDisable()
        // {
        //     EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
        //     EventManager.Units.onUnitSpawned -= OnUnitSpawned;
        // }
    }
}