using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class Util : MonoBehaviour
{

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
}
}