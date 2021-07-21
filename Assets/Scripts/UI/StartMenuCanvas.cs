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


    [Header("Title Sequence")]
    [SerializeField] private CanvasGroup title_01;
    [SerializeField] private CanvasGroup title_02;
    [SerializeField] private CanvasGroup tapCTA;
    public static bool titleAnimCompleted = false;


    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        tapCTA.alpha = 0;
        AnimateTitle(title_01, 1f);
        AnimateTitle(title_02, 1, 0.6f);

        LeanTween.delayedCall(2, () => {
            titleAnimCompleted = true;
            tapCTA.alpha = 1;
        });
    }

    private void AnimateTitle(CanvasGroup title, float duration, float delay=0)
    {
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
            EventManager.Game.onTitleAnimCompleted?.Invoke();
        });
    }

    private void OnTouchBegan(Vector3 touchPos)
    {
               
        if (GameManager.Instance.gameStates.IsStartMenuState() && titleAnimCompleted)
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


    private void OnTitleAnimCompleted()
    {
        var shake = menuPanel.GetComponent<ObjectShake>();
        shake.Shake(menuPanel.gameObject, 0.2f, 3.0f);
    }

    private void OnEnable()
    {
        EventManager.Input.onTouchBegan += OnTouchBegan;
        EventManager.Game.onTitleAnimCompleted += OnTitleAnimCompleted;
    }

    private void OnDisable()
    {
        EventManager.Input.onTouchBegan -= OnTouchBegan;
        EventManager.Game.onTitleAnimCompleted -= OnTitleAnimCompleted;
    }
}
}