using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using BioTower.UI;
using System;

namespace BioTower
{
    public enum PlacementState
    {
        NONE,
        PLACING,
    }

    public class PlacementManager : MonoBehaviour
    {
        [SerializeField] private PlacementState placementState;
        [SerializeField] private LayerMask socketLayerMask;
        private StructureType structureToPlace;
        [SerializeField] private Vector3 placementOffset;



        [Header("Structure prefabs")]
        [SerializeField] private GameObject abaTowerPrefab;
        [SerializeField] private GameObject ppc2TowerPrefab;
        [SerializeField] private GameObject chloroplastTowerPrefab;
        [SerializeField] private GameObject mitoTowerPrefab;
        [SerializeField] private GameObject dnaBasePrefab;


        private void Awake()
        {
            structureToPlace = StructureType.NONE;
        }

        public GameObject CreateStructure(StructureType structureType)
        {
            GameObject tower = null;
            switch (structureType)
            {
                case StructureType.ABA_TOWER:
                    tower = Instantiate(abaTowerPrefab);
                    break;
                case StructureType.PPC2_TOWER:
                    tower = Instantiate(ppc2TowerPrefab);
                    break;
                case StructureType.CHLOROPLAST:
                    tower = Instantiate(chloroplastTowerPrefab);
                    break;
                case StructureType.MITOCHONDRIA:
                    tower = Instantiate(mitoTowerPrefab);
                    break;
                case StructureType.DNA_BASE:
                    tower = Instantiate(dnaBasePrefab);
                    break;
            }

            var structure = tower.GetComponent<Structure>();
            return tower;
        }

        private void OnPressTowerButton(StructureType structureType)
        {
            SetPlacingState(structureType);
        }

        private void OnTapStructureSocket(StructureSocket socket)
        {
            if (!InputController.canSpawnTowers)
                return;

            if (!IsPlacingState())
                return;

            if (socket.HasStructure() || socket.IsBuildingStructure())
                return;

            if (GameManager.Instance.econManager.CanBuyTower(structureToPlace))
            {
                var towerType = structureToPlace;
                var targetSocket = socket;
                EventManager.Structures.onStructureCooldownStarted?.Invoke(towerType, Util.cooldownManager.structureSpawnCooldown);
                GameManager.Instance.econManager.BuyTower(towerType);
                targetSocket.SetIsBuildingStructure();
                targetSocket.StartProgress(Util.gameSettings.towerConstructionDelay);
                LeanTween.delayedCall(gameObject, Util.gameSettings.towerConstructionDelay, () =>
                {
                    PlaceTower(towerType, targetSocket.transform.position, targetSocket);
                });
            }
            SetNoneState();
        }
        private Collider2D DoRaycast(Vector3 screenPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, socketLayerMask);
            return hitInfo.collider;
        }

        private StructureSocket GetSocket(Collider2D collider)
        {
            var socket = collider.transform.parent.GetComponent<StructureSocket>();
            return socket;
        }

        private void PlaceTower(StructureType structureType, Vector3 worldPosition, StructureSocket socket)
        {
            if (socket != null)
                socket.SetHasStructure();

            var tower = CreateStructure(structureType);
            tower.transform.position = worldPosition + placementOffset;
            var structure = tower.GetComponent<Structure>();
            structure.Init(socket);
        }

        private void OnStartPlacementState(StructureType structureType)
        {
            var tower = CreateStructure(structureType);
        }

        public void SetNoneState()
        {
            if (TutorialCanvas.tutorialInProgress && Util.tutCanvas.IsWaitingTapState() || Util.tutCanvas.IsWaitingButtonTapState())
            {

            }
            else
            {
                placementState = PlacementState.NONE;
                structureToPlace = StructureType.NONE;
                EventManager.Structures.onSetNonePlacementState?.Invoke();
            }
        }

        public void SetPlacingState(StructureType structureType)
        {
            placementState = PlacementState.PLACING;
            structureToPlace = structureType;
            EventManager.Structures.onStartPlacementState?.Invoke(structureType);
        }

        public bool IsNoneState() { return placementState == PlacementState.NONE; }
        public bool IsPlacingState() { return placementState == PlacementState.PLACING; }
        public PlacementState GetState() { return placementState; }
        private void ActivateDnaBase()
        {
            var structuresContainer = GameObject.FindGameObjectWithTag(Constants.structuresContainer);
            if (structuresContainer != null)
            {
                var dnaBase = structuresContainer.GetComponentInChildren<DNABase>(true);
                if (dnaBase != null)
                {
                    dnaBase.gameObject.SetActive(true);
                }
            }
        }

        private void ActivateMiniChloro(TutorialData data = null)
        {
            var structuresContainer = GameObject.FindGameObjectWithTag(Constants.structuresContainer);
            if (structuresContainer == null)
                return;

            var miniChloros = structuresContainer.GetComponentsInChildren<MiniChloroplastTower>(true);

            if (data != null)
            {
                bool activateChloros = data.highlightType == HighlightType.NONE;
                bool highlightChloros = data.highlightType == HighlightType.ARROW;

                if (activateChloros)
                {
                    foreach (MiniChloroplastTower tower in miniChloros)
                    {
                        tower.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                foreach (MiniChloroplastTower tower in miniChloros)
                {
                    tower.gameObject.SetActive(true);
                }
            }
        }

        private void OnTutorialStart(TutorialData data)
        {
            if (data.highlightedItem == HighlightedItem.DNA_BASE)
            {
                LeanTween.delayedCall(gameObject, 1.5f, () =>
                {
                    ActivateDnaBase();
                });
            }
            else if (data.highlightedItem == HighlightedItem.MINI_CHLORO)
            {
                ActivateMiniChloro(data);
            }
        }

        private void OnSkipTutorials()
        {
            if (GameManager.Instance.playerBase == null)
                ActivateDnaBase();

            if (LevelInfo.current.levelType == LevelType.LEVEL_02)
                ActivateMiniChloro();
        }

        private void OnEnable()
        {
            EventManager.UI.onPressTowerButton += OnPressTowerButton;
            EventManager.Structures.onTapFreeStructureSocket += OnTapStructureSocket;
            EventManager.Tutorials.onTutorialStart += OnTutorialStart;
            EventManager.Tutorials.onSkipTutorials += OnSkipTutorials;
        }

        private void OnDisable()
        {
            EventManager.UI.onPressTowerButton -= OnPressTowerButton;
            EventManager.Structures.onTapFreeStructureSocket -= OnTapStructureSocket;
            EventManager.Tutorials.onTutorialStart -= OnTutorialStart;
            EventManager.Tutorials.onSkipTutorials -= OnSkipTutorials;
        }

    }
}