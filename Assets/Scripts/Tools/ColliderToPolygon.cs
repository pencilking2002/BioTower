using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Shapes;

public class ColliderToPolygon : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D col;
    [SerializeField] private Polygon polygon;

    [Button("Bake")]
    private void Bake()
    {
        var points = col.points;
        polygon.SetPoints(points);
    }

    [Button("Reset Polygon")]
    private void ResetPolygon()
    {
        var arr = new Vector2[0];
        polygon.SetPoints(arr);
    }
}
