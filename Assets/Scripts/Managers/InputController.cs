using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    public static Action<Vector3> onTouchBegan;     // screen position of the touch

    private void Update()
    {
        #if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            onTouchBegan?.Invoke(Input.mousePosition);
        }
        
        #elif UNITY_ANDROID

        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                onTouchBegan?.Invoke(touch.position);
            }
        }

        #endif
    }
    

}
