using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using BioTower.Units;
using PolyNav;

namespace BioTower.Structures
{
[SelectionBase]
public class PPC2Tower : Structure
{
    [SerializeField] private float discRotateSpeed = 2;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootDuration = 1.0f;
    [SerializeField] private float shootInterval;
    private float lastShotTime;


    [Header("References")]
    [SerializeField] private List<Unit> units;
    [SerializeField] private Disc influenceDisc;
    [SerializeField] private CircleCollider2D maxInfluenceCollider;
    [SerializeField] private CircleCollider2D minInfluenceCollider;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitsContainer;
    //public PolyNav2D map;

    private void Awake()
    {
        Util.ScaleUpSprite(sr, 1.1f);
    }

    public override void OnUpdate()
    {
        influenceDisc.transform.eulerAngles += new Vector3(0,0,discRotateSpeed * Time.deltaTime);
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

        // Get the enemy's move direction
        BasicEnemy enemy = other.transform.parent.GetComponent<BasicEnemy>();
        Vector2 enemyMoveDirection = enemy.agent.movingDirection.normalized;
        endPos += (Vector3) enemyMoveDirection * UnityEngine.Random.Range(0.1f, 0.5f);

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

    public Vector2 GetPointWithinInfluence()
    {
        return Util.GetPointWithinInfluence(
                minInfluenceCollider.transform.position, 
                minInfluenceCollider.radius, 
                maxInfluenceCollider.radius
            );
    }

     public override void SpawnUnits(int numUnits)
    {
        for(int i=0; i<numUnits; i++)
        {
            var go = Instantiate(unitPrefab);
            //go.transform.position = GetPointWithinInfluence();
            go.transform.position = GetPointWithinInfluence(); 
                
            go.transform.SetParent(unitsContainer);
            var unit = go.GetComponent<Snrk2Unit>();
            unit.agent.map = GameManager.Instance.levelMap.map; 
            unit.tower = this;
            AddUnit(unit);
        }
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
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
