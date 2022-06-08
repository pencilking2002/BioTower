using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using UnityEngine.EventSystems;

namespace BioTower
{
    public class TapManager : MonoBehaviour
    {
        [SerializeField] private LayerMask tappableLayerMask;
        [SerializeField] private LayerMask structureLayerMask;
        public Structure selectedStructure;
        public bool hasSelectedStructure;


        private void OnTouchBegan(Vector3 screenPos)
        {
            if (!GameManager.Instance.gameStates.IsGameState())
                return;

            bool didTapSomething = false;
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, tappableLayerMask);
            if (hitInfo.collider != null)
            {
                //TapCrystal(hitInfo);
                didTapSomething = true;
                TapLightFragment(hitInfo);
                TapStructureSocket(hitInfo);
            }

            bool didFindStructure = TapStructure(screenPos);
            bool didHitUI = DidHitUI(screenPos);

            if (didFindStructure || didHitUI)
                didTapSomething = true;

            if (!didTapSomething)
                EventManager.Input.onTapNothing?.Invoke();
        }

        private bool DidHitUI(Vector3 screenPos)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = screenPos;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == Util.uiLayer)
                {
                    return true;
                }
            }
            return false;
        }

        // private void TapCrystal(RaycastHit2D hitInfo)
        // {
        //     EnemyCrystal crystal = hitInfo.collider.transform.parent.GetComponent<EnemyCrystal>();
        //     if (crystal != null)
        //     {
        //         crystal.DestroyObject();
        //         EventManager.Game.onCrystalTapped?.Invoke();
        //         //GameManager.Instance.econManager.GainCrystalMoney();
        //     }
        // }

        private void TapLightFragment(RaycastHit2D hitInfo)
        {
            if (Util.upgradeSettings.numFragmentsPickedUpOnTap == 1)
            {
                LightFragment fragment = hitInfo.collider.transform.parent.GetComponent<LightFragment>();
                if (fragment != null)
                {
                    fragment.DestroyObject();
                }
            }
            else if (Util.upgradeSettings.numFragmentsPickedUpOnTap > 1)
            {
                var radius = Util.gameSettings.multipleLightFragmentPickupRadius;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(hitInfo.point, radius, tappableLayerMask);
                if (colliders.Length > 0)
                {
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (i > Util.upgradeSettings.numFragmentsPickedUpOnTap)
                            continue;

                        LightFragment fragment = colliders[i].transform.parent.GetComponent<LightFragment>();
                        LeanTween.move(fragment.gameObject, hitInfo.point, 0.15f)
                            .setOnComplete(() =>
                            {
                                fragment.DestroyObject();
                                EventManager.Game.onLightFragmentTapped?.Invoke();
                            });
                    }
                }
            }
        }

        private void TapStructureSocket(RaycastHit2D hitInfo)
        {
            if (hitInfo.collider.gameObject.layer == Util.structureSocketLayer)
            {
                var socket = hitInfo.collider.transform.parent.GetComponent<StructureSocket>();
                socket.OnTap();
            }
        }

        private bool TapStructure(Vector3 screenPos)
        {
            if (GameManager.Instance.placementManager.IsNoneState())
            {
                Ray ray = Camera.main.ScreenPointToRay(screenPos);
                RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, structureLayerMask);

                if (hitInfo.collider != null)
                {
                    var structure = hitInfo.collider.transform.parent.GetComponent<Structure>();
                    hasSelectedStructure = true;
                    selectedStructure = structure;
                    EventManager.Structures.onStructureSelected?.Invoke(structure);
                    return true;
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(screenPos);
                RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity, structureLayerMask);

                if (hitInfo.collider != null)
                {
                    var structure = hitInfo.collider.transform.parent.GetComponent<Structure>();
                    hasSelectedStructure = true;
                    selectedStructure = structure;
                    EventManager.Structures.onStructureSelected?.Invoke(structure);
                    return true;
                    //GameManager.Instance.placementManager.SetPlacingState(selectedStructure.structureType);
                }
            }
            return false;
        }

        private void OnEnable()
        {

            EventManager.Input.onTouchBegan += OnTouchBegan;

        }

        private void OnDisable()
        {

            EventManager.Input.onTouchBegan -= OnTouchBegan;
        }
    }
}