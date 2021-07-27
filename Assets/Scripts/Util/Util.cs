using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector2 point = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
        point += centerPoint;
        return point;
    }

}
}