using UnityEngine;
using System;

namespace BioTower
{
public class InputController : MonoBehaviour
{
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
    }
}
}