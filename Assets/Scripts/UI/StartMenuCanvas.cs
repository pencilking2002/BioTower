using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class StartMenuCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuPanel;
    [SerializeField] private RectTransform innerMenuPanel;
    [SerializeField] private float targetYOffset = 50;
    [SerializeField] private float animDuration = 1.0f;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void OnTouchBegan(Vector3 touchPos)
    {
               
        if (GameManager.Instance.gameStates.IsStartMenuState())
        {
            EventManager.Input.onTapStartMenu?.Invoke();
            Vector3 localPos = innerMenuPanel.localPosition;

            var seq = LeanTween.sequence();

            seq.append(() => {
                LeanTween.moveLocalY(innerMenuPanel.gameObject, localPos.y + targetYOffset, animDuration).setEaseOutSine();
            });
            
            seq.append(animDuration/2);

            seq.append(LeanTween.alphaCanvas(menuPanel, 0, animDuration));

            seq.append(() => {
                canvas.enabled = false;
            });
        }
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