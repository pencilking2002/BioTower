using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace BioTower
{
public class InputController : MonoBehaviour
{
    public GameObject lastSelected;
    
    private void Update()
    {
        #if UNITY_EDITOR || UNITY_WEBGL

        if (Input.GetMouseButtonDown(0))
        {
            EventManager.Input.onTouchBegan?.Invoke(Input.mousePosition);

            // var evData = new PointerEventData(EventSystem.current);
            // evData.position = Input.mousePosition;
            // List<RaycastResult> results = new List<RaycastResult>();
            // GameManager.Instance.bootController.gameCanvas.GetComponent<GraphicRaycaster>().Raycast(evData, results);
            // if (results.Count == 0)
            //     EventManager.Input.onTap?.Invoke();
        }
        
        #elif UNITY_ANDROID

        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                EventManager.Input.onTouchBegan?.Invoke(touch.position);
                EventManager.Input.onTap?.Invoke();
            }
        }

        #endif

        HandleButtonDeselect();

    }

    private void HandleButtonDeselect()
    {
        if (GameManager.Instance.gameStates.IsGameState() || GameManager.Instance.gameStates.IsUpgradeMenuState())
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                if (lastSelected != null && lastSelected.activeSelf && lastSelected.GetComponent<Button>() != null && lastSelected.GetComponent<Button>().interactable)
                {
                    EventSystem.current.SetSelectedGameObject(lastSelected);
                }            
            }
            else
            {
                lastSelected = EventSystem.current.currentSelectedGameObject;
            }
        }
    }
}
}