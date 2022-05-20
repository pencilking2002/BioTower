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
        [SerializeField] protected UnitCounterCanvas unitCounterCanvas;
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

            towerAlert.OnUpdate();
            unitCounterCanvas.OnUpdate(units.Count);
        }

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

        public override void OnStructureSelected(Structure structure)
        {
            if (TutorialCanvas.tutorialInProgress)
            {
                if (Util.tutCanvas.tutState != TutState.WAITING_BUTTON_TAP)
                {
                    //Debug.Log("Tut state: " + Util.tutCanvas.tutState);
                    return;
                }
            }

            base.OnStructureSelected(structure);
            if (structure == this)
            {
                Util.bootController.towerMenu.OnPressSpawnUnitButton();
            }
        }

        public override void OnHighlightItem(HighlightedItem item)
        {
            if (Util.tutCanvas.skipTutorials)
                return;

            if (LevelInfo.current.IsFirstLevel() && item == HighlightedItem.ABA_UNIT_BTN)
            {
                var worldPos = sr.transform.position;
                Util.poolManager.SpawnItemHighlight(worldPos, new Vector2(0, 250));
            }
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

