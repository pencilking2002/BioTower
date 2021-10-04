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
    [SerializeField] private BoxCollider2D lightFragmentAvoidanceCollider;
    [SerializeField] private float shootDuration = 1.0f;
    [SerializeField] private float shootInterval = 5;
    private float lastShotTime;

    public override void Awake()
    {
        base.Awake();
        shootInterval = Util.upgradeSettings.chloroShootInterval_float.GetFloat();
        lastShotTime = Time.time;
    }

    private GameObject CreateFragment()
    {
        //GameObject fragment = Instantiate(lightFragmentPrefab); 
        PooledObject obj = Util.poolManager.GetPooledObject(PoolObjectType.LIGHT_FRAGMENT);
        return obj.gameObject;
    }

    public void Update()
    {
        if (Time.time > lastShotTime + shootInterval)
        {
            ShootFragment();
            lastShotTime = Time.time;
        }
    }

    [Button("Shoot Fragment")]
    private void ShootFragment(bool avoidFragmentCollider=true)
    {
        var fragment = CreateFragment();
        Vector3 startPos = transform.position;
        Vector3 endPos = GetPointWithinInfluence(avoidFragmentCollider);
        Vector3 controlPoint = startPos + (endPos-startPos) * 0.5f + Vector3.up;
        fragment.transform.position = startPos;

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

    private Vector2 GetRandomPoint()
    {
        return Util.GetPointWithinInfluence(
            minInfluenceCollider.transform.position, 
            minInfluenceCollider.radius, 
            maxInfluenceCollider.radius
        );
    }

    public Vector2 GetPointWithinInfluence(bool avoidFragmentCollider)
    {
        Vector2 randomPoint;

        if (avoidFragmentCollider)
        {
            do randomPoint = GetRandomPoint();
            while (lightFragmentAvoidanceCollider.OverlapPoint(randomPoint));
        }
        else
        {
            randomPoint = GetRandomPoint();
        }

        return randomPoint;
    }

   

    public override void KillStructure()
    {
        for (int i=0; i<10; i++)
        {
            ShootFragment(false);
        }

        sr.gameObject.SetActive(false);
        isAlive = false;
        GameManager.Instance.CreateTowerExplosion(transform.position);

        LeanTween.delayedCall(2.0f, () => {
            EventManager.Structures.onStructureDestroyed?.Invoke(this);
            if (socket != null)
                socket.SetHasStructure(false);
            
            Destroy(gameObject);
        });
    }
}
}