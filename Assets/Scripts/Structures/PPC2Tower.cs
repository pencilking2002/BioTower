using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Structures
{
[SelectionBase]
public class PPC2Tower : Structure
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private CircleCollider2D maxInfluenceAreaCollider;
    [SerializeField] private float shootDuration = 1.0f;
    [SerializeField] private float shootDelay = 0.1f;


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

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("PPC2 twoerr overlap");
        HandleEnemyOverlap(col);
    }

    private void HandleEnemyOverlap(Collider2D col)
    {
        if (col.gameObject.layer != 10)
            return;

        LeanTween.delayedCall(gameObject, shootDelay, () => {
            var projectile = CreateProjectile();
            projectile.transform.position = transform.position;
            projectile.transform.right = (col.transform.position-transform.position).normalized;
            
            Vector3 targetPos = col.transform.position;
            LeanTween.move(projectile.gameObject, targetPos, shootDuration)
            .setOnComplete(projectile.Explode);
        }); 
    }

}
}
