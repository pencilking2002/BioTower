using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using PolyNav;

namespace BioTower.Structures
{

    [SelectionBase]
    public class ABATower : Structure
    {
        [SerializeField] private float discRotateSpeed = 2;
        [SerializeField] private int numUnitsToSpawn = 4;
        [SerializeField] private List<EnemyUnit> enemiesWithinInfluence;


        [Header("References")]
        [SerializeField] private GameObject abaUnitPrefab;
        public PolyNav2D map;
        [SerializeField] private Transform unitsContainer;
        [SerializeField] private CircleCollider2D maxInfluenceAreaCollider;
        [SerializeField] private CircleCollider2D minInfluenceAreaCollider;


        public override void Awake()
        {
            base.Awake();
            map.GenerateMap(true);
        }

        private void Start()
        {
            influenceVisuals.gameObject.SetActive(true);
            var radius = Util.upgradeSettings.abaMaxInfluenceRadius_float.GetFloat();
            influenceVisuals.Radius = radius;
            maxInfluenceAreaCollider.radius = radius;
            map.transform.localScale = Vector3.one * radius;

            map.GenerateMap();      // NOTE: Seems like this needs to be called in order for the map to be initialized correctly after instantiation 
            var unitsContainer = transform.Find("Units");

            SpawnUnits(numUnitsToSpawn);
        }

        public override void Init(StructureSocket socket)
        {
            base.Init(socket);
        }

        public override void OnUpdate()
        {
            if (!isAlive || GameManager.Instance.gameStates.gameState != GameState.GAME)
                return;

            if (influenceVisuals != null)
                influenceVisuals.transform.eulerAngles += new Vector3(0, 0, discRotateSpeed * Time.deltaTime);

            towerAlert.OnUpdate(this);
            //ChaseEnemies();
        }

        // private void ChaseEnemies()
        // {
        //     for (int i = 0; i < units.Count; i++)
        //     {
        //         AbaUnit abaUnit = (AbaUnit)units[i];
        //         EnemyUnit enemy = (EnemyUnit)abaUnit.unitFoe;

        //         if (abaUnit.IsChasingState())
        //         {
        //             if (enemy.IsCombatState())
        //             {
        //                 abaUnit.SetRoamingState();
        //                 abaUnit.unitFoe = null;
        //             }
        //             else if (enemy.IsChasingState())
        //             {
        //                 if (enemy.unitFoe == this)
        //                 {
        //                     var enemyPos = enemy.transform.position;
        //                     var abaPos = abaUnit.transform.position;
        //                     abaUnit.SetDestination(enemyPos);
        //                     enemy.SetDestination(abaPos);
        //                 }
        //                 // else
        //                 // {
        //                 //     abaUnit.SetRoamingState();
        //                 // }
        //             }
        //         }
        //         else if (abaUnit.IsRoamingState())
        //         {
        //             var roamingEnemy = FindClosestRoamingEnemy(abaUnit, out bool isFound);

        //             if (isFound)
        //             {
        //                 if (!roamingEnemy.IsChasingState())
        //                     roamingEnemy.SetChasingState(abaUnit);

        //                 abaUnit.SetChasingState(roamingEnemy);
        //             }
        //         }
        //     }
        // }

        public EnemyUnit FindClosestRoamingEnemy(Unit abaUnit, out bool isFound)
        {
            isFound = false;
            EnemyUnit closestEnemy = null;
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < enemiesWithinInfluence.Count; i++)
            {
                var enemy = enemiesWithinInfluence[i];
                if (enemy.IsRoamingState())
                {
                    float distance = Vector2.Distance(enemy.transform.position, abaUnit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                        isFound = true;
                    }
                }
            }
            return closestEnemy;
        }

        public override void SpawnUnits(int numUnits)
        {
            for (int i = 0; i < numUnits; i++)
            {
                var go = Instantiate(abaUnitPrefab);
                go.transform.position = GetEdgePointWithinInfluence();

                go.transform.SetParent(unitsContainer);
                var unit = go.GetComponent<AbaUnit>();
                unit.agent.map = map;
                unit.tower = this;

                var scale = unit.transform.localScale;
                unit.transform.localScale = Vector3.zero;

                unitSpawnTrail.emitting = true;
                unitSpawnTrail.transform.localPosition = Vector2.zero;
                AddUnit(unit);
                var seq = LeanTween.sequence();
                LTSpline ltSpline = new LTSpline(
                    new Vector3[] {
                        new Vector3(UnityEngine.Random.Range(0,1), UnityEngine.Random.Range(0,1), 0f),
                        transform.position,
                        unit.transform.position,
                        new Vector3(UnityEngine.Random.Range(0,1), UnityEngine.Random.Range(0,1), 0f)
                    });
                seq.append(LeanTween.moveSpline(unitSpawnTrail.gameObject, ltSpline, 0.1f));

                seq.append(LeanTween.scale(unit.gameObject, scale * 2, 0.1f));
                seq.append(LeanTween.scale(unit.gameObject, scale, 0.25f));
                seq.append(() =>
                {
                    if (unitSpawnTrail)
                    {
                        //Debug.Break();
                        unitSpawnTrail.emitting = false;
                        unitSpawnTrail.transform.localPosition = Vector2.zero;
                    }
                });
            }
        }

        public void AddUnit(AbaUnit unit)
        {
            units.Add(unit);
        }

        public void RemoveUnit(AbaUnit unit)
        {
            units.Remove(unit);
        }

        public Vector2 GetEdgePointWithinInfluence()
        {
            if (minInfluenceAreaCollider == null || maxInfluenceAreaCollider == null)
                return Vector2.zero;

            var point = Util.GetPointWithinInfluence(
                    minInfluenceAreaCollider.transform.position,
                    minInfluenceAreaCollider.radius,
                    maxInfluenceAreaCollider.radius
                );
            Vector2 center = minInfluenceAreaCollider.transform.position;
            Vector2 direction = (point - center).normalized;
            point = center + direction * maxInfluenceAreaCollider.radius;
            return point;
        }

        private void OnDrawGizmosSelected()
        {
            var minRadius = minInfluenceAreaCollider.radius * minInfluenceAreaCollider.transform.lossyScale.x;
            var maxRadius = maxInfluenceAreaCollider.radius * maxInfluenceAreaCollider.transform.lossyScale.x;

            var color = Color.red;
            color.a = 0.2f;
            Gizmos.color = color;
            Gizmos.DrawSphere(minInfluenceAreaCollider.transform.position, minRadius);

            color = Color.blue;
            color.a = 0.2f;
            Gizmos.color = color;
            Gizmos.DrawSphere(maxInfluenceAreaCollider.transform.position, maxRadius);
        }

        public void RegisterEnemy(EnemyUnit enemy)
        {
            if (!HasEnemy(enemy))
            {
                enemiesWithinInfluence.Add(enemy);

                // if (enemy.IsRoamingState())
                // {
                //     var abaUnit = GetClosestUnit(enemy, out bool unitFound);

                //     if (unitFound && abaUnit.IsRoamingState())
                //     {
                //         enemy.SetChasingState(abaUnit);
                //         abaUnit.SetChasingState(enemy);
                //     }
                // }

                //EventManager.Structures.onEnemyEnterTowerInfluence?.Invoke(enemy, this);
            }
        }

        public bool HasEnemy(EnemyUnit enemy)
        {
            return enemiesWithinInfluence.Contains(enemy);
        }

        public bool IsBelowSpawnLimit()
        {
            return units.Count < GameManager.Instance.upgradeSettings.abaUnitSpawnLimit;
        }

        public void UnregisterEnemy(EnemyUnit enemy)
        {
            if (enemiesWithinInfluence.Contains(enemy))
            {
                // Set them back to roaming
                enemiesWithinInfluence.Remove(enemy);
                EventManager.Structures.onEnemyExitTowerInfluence?.Invoke(enemy, this);
            }
        }

    }
}

