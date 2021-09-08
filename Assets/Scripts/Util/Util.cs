using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace BioTower
{
public class Util : MonoBehaviour
{
    public static GameSettings gameSettings => GameManager.Instance.gameSettings;
    public static CrystalManager crystalManager => GameManager.Instance.crystalManager;
    public LayerMask enemyLayerMask;
    
    public static void ScaleBounceSprite(SpriteRenderer sr, float scaleUpFactor)
    {
        var oldScale = sr.transform.localScale;
        LeanTween.scale(sr.gameObject, oldScale * scaleUpFactor, 0.1f).setOnComplete(() => {
             LeanTween.scale(sr.gameObject, oldScale, 0.1f);
        }); 
    }

    public static void ScaleUpSprite(SpriteRenderer sr, float scaleUpFactor)
    {
        var oldScale = sr.transform.localScale;
        sr.transform.localScale = Vector3.zero;
        LeanTween.scale(sr.gameObject, oldScale * scaleUpFactor, 0.1f).setOnComplete(() => {
             LeanTween.scale(sr.gameObject, oldScale, 0.1f);
        }); 
    }

    // public static void AnimateBounceSpriteColor(SpriteRenderer sr, Color color)
    // {
    //     var oldColor = sr.color;
    //     LeanTween.value(sr.gameObject, oldColor, Color.red, 0.1f).setOnComplete(() => {
    //         sr.color = 
    //     });
    // }

    public static Vector2 Bezier2(Vector2 start,  Vector2 control, Vector2 end, float t)
    {
        return (((1-t)*(1-t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
    }

    public static Vector2 GetPointWithinInfluence(Vector2 centerPoint, float minRadius, float maxRadius)
    {
        Vector2 point = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(minRadius, maxRadius);
        point += centerPoint;
        return point;
    }

    public enum ButtonColorMode
    {
        DEFAULT,
        SELECTED
    }

    public static void HandleInvalidButtonPress(Button button, ButtonColorMode colorMode=ButtonColorMode.SELECTED)
    {
        if (colorMode == ButtonColorMode.SELECTED)
        {
            var colors = button.colors;
            Color oldColor = colors.selectedColor;
            colors.selectedColor = Color.red;
            button.colors = colors;
            LeanTween.delayedCall(0.15f, () => {
                colors.selectedColor = oldColor;
                button.colors = colors;
            });
        }
        else
        {
            var image = button.GetComponent<Image>();
            Color oldColor = image.color;
            image.color = Color.red;
            LeanTween.delayedCall(0.15f, () => {
                image.color = oldColor;
            });
        }
        EventManager.UI.onTapButton?.Invoke(false);
    }

    public void TextReveal(TMP_Text text, float revealDuration, Action onComplete=null)
    {
        StartCoroutine(RevealCharacters(text, revealDuration, onComplete));
    }

    private IEnumerator RevealCharacters(TMP_Text textComponent, float revealDuration, Action onComplete=null)
    {
        textComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = textComponent.textInfo;
        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        while (visibleCount <= totalVisibleCharacters)
        {
            textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
            visibleCount += 1;
            EventManager.UI.onLetterReveal?.Invoke();
            yield return new WaitForSeconds(revealDuration);
        }

        if (visibleCount >= totalVisibleCharacters)
        {
            onComplete?.Invoke();
            yield return null;
        }
    }

}
}