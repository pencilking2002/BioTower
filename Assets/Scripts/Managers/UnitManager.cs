using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Units
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private List<Unit> units;

        public void Register(Unit unit)
        {
            units.Add(unit);
        }

        public void Unregister(Unit unit)
        {
            units.Remove(unit);
        }

        private void Update()
        {
            if (!GameManager.Instance.gameStates.IsGameState())
                return;

            HandleUnitFacingDirections();
            HandleSnrk2CrystalChecking();
            HandleEnemyUnitDestinationChecks();
        }

        private void HandleUnitFacingDirections()
        {
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null || !unit.isAlive)
                    continue;

                Vector2 moveDir = unit.agent.movingDirection;

                if (moveDir.sqrMagnitude > 0.1f)
                {
                    if (!ReferenceEquals(unit, null) && !ReferenceEquals(unit.sr, null))
                    {
                        float dot = Vector2.Dot(moveDir.normalized, Vector2.right) * 0.5f + 0.5f;
                        bool faceRight = dot > 0.5f;
                        unit.sr.flipX = faceRight;
                    }
                }
            }
        }

        private void HandleSnrk2CrystalChecking()
        {
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit != null && unit.unitType == UnitType.SNRK2)
                {
                    var snrk2 = (Snrk2Unit)unit;
                    if (snrk2.IsRoamingState())
                        snrk2.CheckForCrystals();
                }
            }
        }

        private void HandleEnemyUnitDestinationChecks()
        {
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit.IsEnemy())
                {
                    var enemy = (EnemyUnit)unit;
                    if (enemy.IsRoamingState())
                    {
                        if (enemy.GetNextWaypoint() != null)
                        {
                            //var destination = enemy.agent.primeGoal;
                            var waypointPosition = enemy.GetNextWaypoint().transform.position;
                            if (Vector3.Distance(enemy.transform.position, waypointPosition) > 0.2f)
                            {
                                Debug.Log(enemy.gameObject.name + " is off destination");
                                enemy.SetDestination(waypointPosition);
                            }
                        }
                        else
                        {
                            Debug.Log("enemy does not have a waypoint to go to");
                        }
                    }
                    else if (enemy.IsChasingState())
                    {
                        if (!enemy.unitFoe || !enemy.unitFoe.isAlive || !enemy.unitFoe.IsChasingState() || enemy.unitFoe.unitFoe != enemy)
                            enemy.SetRoamingState();
                        else
                        {
                            var destination = enemy.unitFoe.transform.position;
                            enemy.SetDestination(destination);
                            enemy.agent.primeGoal = destination;
                            Debug.DrawLine(enemy.transform.position, enemy.unitFoe.transform.position, Color.black);
                        }
                        //Debug.Log("Chase aba");
                    }
                }
                else if (unit.IsAba())
                {
                    if (unit.IsChasingState())
                    {
                        var destination = unit.unitFoe.transform.position;
                        unit.SetDestination(destination);
                        unit.agent.primeGoal = destination;
                    }
                }
            }
        }

        private void OnGameOver(bool isWin)
        {
            units.Clear();
        }

        private void OnEnable()
        {
            EventManager.Game.onGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            EventManager.Game.onGameOver -= OnGameOver;

        }

    }
}