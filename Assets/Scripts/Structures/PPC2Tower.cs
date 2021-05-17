using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Structures
{
    
public class PPC2Tower : Structure
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private CircleCollider2D maxInfluenceAreaCollider;

    private void CreateProjectile()
    {
        var projectile = Instantiate(projectilePrefab);
        projectile.transform.SetParent(GameManager.Instance.projectilesContainer);
    }

    private void ShootProjectile(GameObject projectile, Vector3 target)
    {
        LeanTween.move(projectile, target, 0.3f)

        .setOnComplete(() => {
            Destroy(projectile);
        });
    }

}
}
