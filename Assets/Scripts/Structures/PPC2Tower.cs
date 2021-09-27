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
    //[SerializeField] private Disc influenceDisc;
    [SerializeField] private CircleCollider2D maxInfluenceCollider;
    [SerializeField] private CircleCollider2D minInfluenceCollider;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitsContainer;
    public PolyNav2D map;

    public override void Awake()
    {
        base.Awake();
        Util.ScaleUpSprite(sr, 1.1f);
    }

    private void Start()
    {
        influenceVisuals.gameObject.SetActive(true);

        // Setup upgrade settings
        maxInfluenceCollider.radius = Util.upgradeSettings.ppc2MaxInfluenceRadius_float.GetFloat();
        map.transform.localScale = Vector3.one * Util.upgradeSettings.ppc2MapScale_float.GetFloat();
        influenceVisuals.transform.localScale = Vector3.one * Util.upgradeSettings.ppc2InfluenceShapeScale_float.GetFloat();
        shootInterval = Util.upgradeSettings.ppc2shootInterval_float.GetFloat();
    }

    public override void OnUpdate()
    {
        if (!isAlive || GameManager.Instance.gameStates.gameState != GameState.GAME)
            return;

        if (influenceVisuals != null)
            influenceVisuals.transform.eulerAngles += new Vector3(0,0,discRotateSpeed * Time.deltaTime);
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
    
    public bool HasUnitsWithinTowerInfluence()
    {
        if (units.Count == 0)
        {
            return false;
        }
        else
        {
            foreach(Unit unit in units)
            {
                if (unit == null)
                    continue;
                    
                bool isWithinInfluence = IsUnitWithinTowerInfluence(unit);
                if (isWithinInfluence)
                    return true;
            }
        }
        return false;
    }
    private bool IsUnitWithinTowerInfluence(Unit unit)
    {
        var distance = Vector2.Distance(maxInfluenceCollider.transform.position, unit.transform.position);
        return distance <= maxInfluenceCollider.radius;
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

    private void OnCrystalCreated(EnemyCrystal crystal)
    {
        foreach(Unit unit in units)
        {
            Snrk2Unit snrkUnit = (Snrk2Unit) unit;
            if (snrkUnit.IsIdleState() || snrkUnit.IsReturniningState())
            {
               if (Util.crystalManager.HasValidCrystals())
                {
                    snrkUnit.SetupUnitToSearchForCrystal();
                    return;
                }
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        EventManager.Game.onCrystalCreated += OnCrystalCreated;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        EventManager.Game.onCrystalCreated -= OnCrystalCreated;
    }
    
}
}
