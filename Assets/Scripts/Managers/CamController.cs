using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
    public class CamController : MonoBehaviour
    {
        [HideInInspector] public Camera cam;
        [SerializeField] ObjectShake objectShake;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        private void OnGameOver(bool isWin, float delay)
        {
            LeanTween.cancel(objectShake.gameObject);
        }

        private void OnEnable()
        {
            EventManager.Game.onGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            EventManager.Game.onGameOver -= OnGameOver;
        }

    }
}