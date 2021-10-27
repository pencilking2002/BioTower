using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class CamController : MonoBehaviour
{
    [HideInInspector]public Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    // /[SerializeField] ObjectShake objectShake;

    // private void OnTitleAnimCompleted()
    // {
    //     objectShake.Shake(gameObject, 0.25f, 10.0f);
    // }

    private void OnEnable()
    {
        //EventManager.Game.onTitleAnimCompleted += OnTitleAnimCompleted;
    }

    private void OnDisable()
    {
        // /EventManager.Game.onTitleAnimCompleted -= OnTitleAnimCompleted;
    }
    
}
}