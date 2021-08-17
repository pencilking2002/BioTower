using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BioTower
{
public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject[] arrows;

    public void DisplayArrows(Vector2[] localPositions)
    {
        HideArrows();

        for (int i=0; i<localPositions.Length; i++)
        {
            var arrow = arrows[i];
            arrow.SetActive(true);
            arrow.transform.localPosition = localPositions[i];
            OscilateArrow(arrow);
        }
    }

    public void HideArrows()
    {
        foreach(GameObject arrow in arrows)
        {
            LeanTween.cancel(arrow);
            arrow.SetActive(false);
        }
    }

    public void OscilateArrow(GameObject arrow)
    {
        var arrowPos = arrow.transform.localPosition;
        LeanTween.moveLocalY(arrow, arrowPos.y - 20, 0.5f).setLoopPingPong(-1);
    }
}
}