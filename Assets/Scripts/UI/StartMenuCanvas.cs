﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class StartMenuCanvas : MonoBehaviour
{
    public CanvasGroup menuPanel;
    [SerializeField] private RectTransform innerMenuPanel;
    [SerializeField] private float targetYOffset = 50;
    [SerializeField] private float animDuration = 1.0f;
    public Canvas canvas;


    [Header("Title Sequence")]
    [SerializeField] private CanvasGroup title_01;
    [SerializeField] private CanvasGroup title_02;
    [SerializeField] private CanvasGroup tapCTA;
    [SerializeField] private CanvasGroup crackPanel_01;
    [SerializeField] private CanvasGroup crackPanel_02;
    public static bool titleAnimCompleted = false;

    private int numTimesTitleDropped;

    private void Awake()
    {
        crackPanel_01.alpha = 0;
        crackPanel_02.alpha = 0;

        tapCTA.alpha = 0;
        AnimateTitle(title_01, 1f);
        AnimateTitle(title_02, 1, 0.6f);

        LeanTween.delayedCall(2, () => {
            titleAnimCompleted = true;
            LeanTween.alphaCanvas(tapCTA, 1, 1).setLoopPingPong(-1);
        });
    }

    private void AnimateTitle(CanvasGroup title, float duration, float delay=0)
    {
        if (title == null)
            return;

        Vector3 oldScale = title.transform.localScale;
        title.transform.localScale = oldScale * 5;
        title.alpha = 0;
        var seq = LeanTween.sequence();
        seq.append(delay);
        seq.append(() => { 
            LeanTween.alphaCanvas(title, 1, duration * 0.5f).setDelay(0.5f);
            LeanTween.scale(title.gameObject, oldScale, duration).setEaseInQuart();
        });
        seq.append(duration);

        seq.append(() => {
            numTimesTitleDropped++;
            EventManager.UI.onTitleAnimCompleted?.Invoke(numTimesTitleDropped);

            if (numTimesTitleDropped == 1)
            {
                LeanTween.value(gameObject, 0, 0.9f, 0.2f).setOnUpdate((float val) => {
                    crackPanel_01.alpha = val;
                });
                var targetScale = crackPanel_01.transform.localScale;
                targetScale.x *= 1.2f;
                LeanTween.scale(crackPanel_01.gameObject, targetScale, 0.05f);
            }
            else
            {
                var targetScale = crackPanel_02.transform.localScale;
                targetScale.x *= 1.2f;
                LeanTween.scale(crackPanel_02.gameObject, targetScale, 0.05f);
                LeanTween.value(gameObject, 0, 0.9f, 0.2f).setOnUpdate((float val) => {
                    crackPanel_02.alpha = val;
                });
            }
        });
    }

    private void OnTouchBegan(Vector3 touchPos)
    {
               
        if (GameManager.Instance.gameStates.IsStartMenuState() && titleAnimCompleted)
        {
            LeanTween.cancel(tapCTA.gameObject);
            tapCTA.alpha = 0;
            
            EventManager.Input.onTapStartMenu?.Invoke();
            Vector3 localPos = innerMenuPanel.localPosition;

            var seq = LeanTween.sequence();

            seq.append(() => {
                LeanTween.moveLocalY(innerMenuPanel.gameObject, localPos.y + targetYOffset, animDuration).setEaseOutSine();
            });
            
            seq.append(animDuration/2);

            seq.append(LeanTween.alphaCanvas(menuPanel, 0, animDuration));

            seq.append(() => {
                if (canvas != null)
                    canvas.enabled = false;
            });
        }
    }


    private void OnTitleAnimCompleted(int numTimes)
    {
        var shake = menuPanel.GetComponent<ObjectShake>();
        shake.Shake(menuPanel.gameObject, 0.2f, 3.0f);
    }

    private void OnEnable()
    {
        EventManager.Input.onTouchBegan += OnTouchBegan;
        EventManager.UI.onTitleAnimCompleted += OnTitleAnimCompleted;
    }

    private void OnDisable()
    {
        EventManager.Input.onTouchBegan -= OnTouchBegan;
        EventManager.UI.onTitleAnimCompleted -= OnTitleAnimCompleted;
    }
}
}