using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using BioTower.Units;

namespace BioTower
{
    public class WaitingButtonTutState : TutStateBase
    {
        public override void Init(TutState tutState)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                StopCoroutine(GameManager.Instance.util.RevealCharacters(null, 0, null));
                tutCanvas.tutText.text = tutCanvas.currTutorial.text;
                tutCanvas.tutText.ForceMeshUpdate();

                InputController.canPressButtons = true;
                InputController.canSpawnTowers = false;

                var animatedWords = tutCanvas.currTutorial.animatedWords;
                foreach (AnimatedWord word in animatedWords)
                    EventManager.Tutorials.onAnimateText?.Invoke(word.word, word.speed, word.amplitude);

                EventManager.Tutorials.onTutStateInit?.Invoke(tutState);
            }
        }

        public override TutState OnUpdate(TutState tutState)
        {
            Init(tutState);
            return tutState;
        }

        private void OnPressTowerButton(StructureType structureType)
        {
            if (!isInitialized)
                return;

            if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
            {
                tutCanvas.SetEndTutState();
                return;
            }

            if (structureType == StructureType.ABA_TOWER)
            {
                if (tutCanvas.currTutorial.requiredAction == RequiredAction.TAP_ABA_TOWER_BUTTON)
                {
                    tutCanvas.SetLetterRevealState();
                }
            }
        }

        // private void OnTapSpawnUnitButton(UnitType unitType)
        // {
        //     if (!isInitialized)
        //         return;

        //     if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
        //     {
        //         tutCanvas.SetEndTutState();
        //         return;
        //     }

        //     if (unitType == UnitType.ABA)
        //     {
        //         if (tutCanvas.currTutorial.IsSpawnAbaUnitRequiredAction())
        //         {
        //             tutCanvas.SetLetterRevealState();
        //         }
        //     }
        // }

        private void OnStructureSelected(Structure structure)
        {
            if (!isInitialized)
                return;

            LeanTween.delayedCall(0.1f, () =>
            {
                if (tutCanvas.IsLastTutorial(tutCanvas.currTutorial))
                {
                    tutCanvas.SetEndTutState();
                    return;
                }

                if (structure.structureType == StructureType.ABA_TOWER)
                {
                    if (tutCanvas.currTutorial.IsSpawnAbaUnitRequiredAction())
                    {
                        tutCanvas.SetLetterRevealState();
                    }
                }
            });
        }


        public override void OnTutStateInit(TutState tutState)
        {
            if (tutState != this.tutState)
                isInitialized = false;
        }

        private void OnEnable()
        {
            EventManager.Tutorials.onTutStateInit += OnTutStateInit;
            EventManager.UI.onPressTowerButton += OnPressTowerButton;
            //EventManager.UI.onTapSpawnUnitButton += OnTapSpawnUnitButton;
            EventManager.Structures.onStructureSelected += OnStructureSelected;
        }

        private void OnDisable()
        {
            EventManager.Tutorials.onTutStateInit -= OnTutStateInit;
            EventManager.UI.onPressTowerButton -= OnPressTowerButton;
            //EventManager.UI.onTapSpawnUnitButton -= OnTapSpawnUnitButton;
            EventManager.Structures.onStructureSelected -= OnStructureSelected;
        }
    }
}