using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BioTower.Structures
{
public class ChloroplastTower : Structure
{
    [SerializeField] private GameObject lightFragmentPrefab;    
    [SerializeField] private CircleCollider2D maxInfluenceCollider;
    [SerializeField] private CircleCollider2D minInfluenceCollider;
    [SerializeField] private float shootDuration = 1.0f;
    [SerializeField] private float shootInterval = 5;
    private float lastShotTime;

    private GameObject CreateFragment()
    {
        GameObject fragment = Instantiate(lightFragmentPrefab); 
        return fragment;
    }

    private void Update()
    {
        if (Time.time > lastShotTime + shootInterval)
        {
            ShootFragment();
            lastShotTime = Time.time;
        }
    }

    [Button("Shoot Fragment")]
    private void ShootFragment()
    {
        var fragment = CreateFragment();
        Vector3 startPos = transform.position;
        Vector3 endPos = GetPointWithinInfluence();
        Vector3 controlPoint = startPos + (endPos-startPos) * 0.5f + Vector3.up;

        var seq = LeanTween.sequence();

        seq.append(
            LeanTween.value(gameObject, 0,1, shootDuration)
            .setOnUpdate((float val) => {
                Vector2 targetPos = Util.Bezier2(startPos, controlPoint, endPos, val);
                fragment.transform.position = targetPos;
            })
            .setEaseInSine()
        );

        seq.append(LeanTween.moveY(fragment, endPos.y + 0.06f, 0.1f));
        seq.append(LeanTween.moveY(fragment, endPos.y, 0.1f));
    }

    public Vector2 GetPointWithinInfluence()
    {
        return Util.GetPointWithinInfluence(
                minInfluenceCollider.transform.position, 
                minInfluenceCollider.radius, 
                maxInfluenceCollider.radius
            );
    }
}
}