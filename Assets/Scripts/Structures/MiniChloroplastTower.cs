using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BioTower.Structures
{
public class MiniChloroplastTower : Structure
{
    [SerializeField] private CircleCollider2D maxInfluenceCollider;
    [SerializeField] private CircleCollider2D minInfluenceCollider;
    [SerializeField] private float shootDuration = 1.0f;
    [SerializeField] private float shootInterval = 3;
    private float lastShotTime;

    public override void Awake()
    {
        base.Awake();
        lastShotTime = Time.time + UnityEngine.Random.Range(0,0.3f);
    }

    private void Start()
    {
        base.Init(null);
    }

    public override void OnUpdate()
    {
        if (Time.time > lastShotTime + shootInterval)
        {
            ShootFragment();
            lastShotTime = Time.time + UnityEngine.Random.Range(0.0f, 1.0f);
        }
    }

    private GameObject CreateFragment()
    {
        PooledObject obj = Util.poolManager.GetPooledObject(PoolObjectType.LIGHT_FRAGMENT);
        return obj.gameObject;
    }


    [Button("Shoot Fragment")]
    private void ShootFragment(bool avoidFragmentCollider=true)
    {
        var scale = sr.transform.localScale;
        var seq = LeanTween.sequence();
        seq.append(LeanTween.scale(sr.gameObject, scale*1.2f, 0.1f));
        seq.append(LeanTween.scale(sr.gameObject, scale, 0.4f).setEaseOutElastic());
     

        var fragment = CreateFragment();
        Vector3 startPos = transform.position;
        Vector3 endPos = GetPointWithinInfluence(avoidFragmentCollider);
        Vector3 controlPoint = startPos + (endPos-startPos) * 0.5f + Vector3.up;
        fragment.transform.position = startPos;

        var seq2 = LeanTween.sequence();

        seq2.append(
            LeanTween.value(gameObject, 0,1, shootDuration)
            .setOnUpdate((float val) => {
                Vector2 targetPos = Util.Bezier2(startPos, controlPoint, endPos, val);
                fragment.transform.position = targetPos;
            })
            .setEaseInSine()
        );

        seq2.append(LeanTween.moveY(fragment, endPos.y + 0.06f, 0.1f));
        seq2.append(LeanTween.moveY(fragment, endPos.y, 0.1f));
        EventManager.Structures.onLightDropped?.Invoke();
    }

    public Vector2 GetPointWithinInfluence(bool avoidFragmentCollider)
    {
        Vector2 randomPoint = GetRandomPoint();
        return randomPoint;
    }

    private Vector2 GetRandomPoint()
    {
        return Util.GetPointWithinInfluence(
            minInfluenceCollider.transform.position, 
            minInfluenceCollider.radius, 
            maxInfluenceCollider.radius
        );
    }
}
}