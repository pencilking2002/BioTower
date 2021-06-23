using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Structures
{
[SelectionBase]
public class PPC2Tower : Structure
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private CircleCollider2D maxInfluenceCollider;
    [SerializeField] private CircleCollider2D minInfluenceCollider;

   // [SerializeField] private LayerMask enemyLayerMask;
    //[SerializeField] private CircleCollider2D maxInfluenceAreaCollider;
    [SerializeField] private float shootDuration = 1.0f;
    //[SerializeField] private float maxShootDelay = 0.1f;
    [SerializeField] private float shootInterval;
    private float lastShotTime;

    public override void Awake()
    {
        base.Awake();
        Util.ScaleUpSprite(sr, 1.1f);
    }

    public override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        // if (Time.time > lastShot + shootInterval)
        // {  
        //     // TODO: Conver to OverlapAll
        //     var enemy = Physics2D.OverlapCircle(maxInfluenceAreaCollider.transform.position, maxInfluenceAreaCollider.radius, enemyLayerMask);

        //     if (enemy != null && enemy.gameObject.activeInHierarchy)
        //     {
        //         var distance = Vector2.Distance(enemy.transform.position, maxInfluenceAreaCollider.transform.position);
        //         if (distance <= maxInfluenceAreaCollider.radius)
        //         {
        //             ShootEnemy(enemy);
        //             lastShot = Time.time;
        //             Debug.Log("Radius length: " + maxInfluenceAreaCollider.radius + ". Distance to enemy: " + distance.ToString());
        //         }
        //     }
        // }

        // if (Time.time > lastShotTime + shootInterval)
        // {
        //     ShootProjectile();
        //     lastShotTime = Time.time;
        // }
    }

    private PPC2Projectile CreateProjectile()
    {
        var projectile = Instantiate(projectilePrefab);
        projectile.transform.SetParent(GameManager.Instance.projectilesContainer);
        return projectile.GetComponent<PPC2Projectile>();
    }

    private void ShootProjectile(Collider2D other)
    {
        var projectile = CreateProjectile();
        Vector3 startPos = transform.position + Vector3.up * 0.6f;
        Vector3 endPos = (Vector2) other.transform.position + Random.insideUnitCircle * 0.2f;
        Vector3 controlPoint = startPos + (endPos-startPos) * 0.5f + Vector3.up;

        var seq = LeanTween.sequence();

        seq.append(
            LeanTween.value(gameObject, 0,1, shootDuration)
            .setOnUpdate((float val) => {
                Vector2 targetPos = Util.Bezier2(startPos, controlPoint, endPos, val);
                projectile.transform.right = (targetPos - (Vector2) projectile.transform.position).normalized;
                projectile.transform.position = targetPos;
                
            })
            .setEaseInSine()
        );

        seq.append(LeanTween.moveY(projectile.gameObject, endPos.y + 0.06f, 0.1f));
        seq.append(LeanTween.moveY(projectile.gameObject, endPos.y, 0.1f));

        seq.append(projectile.Explode);
    }

    // private void ShootEnemy(Collider2D col)
    // {
    //     var delay = UnityEngine.Random.Range(0, maxShootDelay);

    //     LeanTween.delayedCall(gameObject, delay, () => {
    //         var projectile = CreateProjectile();
    //         projectile.transform.position = transform.position + new Vector3(0,1,0);
    //         projectile.transform.right = (col.transform.position-projectile.transform.position).normalized;

    //         Vector3 targetPos = col.transform.position;
    //         LeanTween.move(projectile.gameObject, targetPos, shootDuration)
    //         .setOnComplete(projectile.Explode);
    //     }); 
    // }

    public Vector2 GetPointWithinInfluence()
    {
        return Util.GetPointWithinInfluence(
                minInfluenceCollider.transform.position, 
                minInfluenceCollider.radius, 
                maxInfluenceCollider.radius
            );
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer != 10)
            return;

        if (Time.time > lastShotTime + shootInterval)
        {   
            ShootProjectile(other);
            lastShotTime = Time.time;
        }
    }
}
}
