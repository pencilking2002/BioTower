using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Structures
{
[SelectionBase]
public class PPC2Tower : Structure
{
    [SerializeField] private GameObject projectilePrefab;
   // [SerializeField] private LayerMask enemyLayerMask;
    //[SerializeField] private CircleCollider2D maxInfluenceAreaCollider;
    [SerializeField] private float shootDuration = 1.0f;
    [SerializeField] private float maxShootDelay = 0.1f;
    //[SerializeField] private float shootInterval;
    //private float lastShot;

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
    }

    private PPC2Projectile CreateProjectile()
    {
        var projectile = Instantiate(projectilePrefab);
        projectile.transform.SetParent(GameManager.Instance.projectilesContainer);
        return projectile.GetComponent<PPC2Projectile>();
    }

    private void ShootProjectile(GameObject projectileGO, Vector3 target)
    {
        var projectile = projectileGO.GetComponent<PPC2Projectile>();
        LeanTween.move(projectileGO, target, 0.3f)
        .setOnComplete(projectile.Explode);
    }

    private void ShootEnemy(Collider2D col)
    {
        var delay = UnityEngine.Random.Range(0, maxShootDelay);

        LeanTween.delayedCall(gameObject, delay, () => {
            var projectile = CreateProjectile();
            projectile.transform.position = transform.position + new Vector3(0,1,0);
            projectile.transform.right = (col.transform.position-projectile.transform.position).normalized;

            Vector3 targetPos = col.transform.position;
            LeanTween.move(projectile.gameObject, targetPos, shootDuration)
            .setOnComplete(projectile.Explode);
        }); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 10)
            return;
        
        ShootEnemy(other);
    }
}
}
