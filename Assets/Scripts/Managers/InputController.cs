using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        }
        
        #elif UNITY_ANDROID

        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                EventManager.Input.onTouchBegan?.Invoke(touch.position);
            }
        }

        #endif

        HandleButtonDeselect();

    }

    private void HandleButtonDeselect()
    {
        if (!GameManager.Instance.gameStates.IsGameState())
            return;

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