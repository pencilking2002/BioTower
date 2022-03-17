using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Units
{
    public class CombatManager : MonoBehaviour
    {
        [Range(0, 100)][SerializeField] private float abaWinChance = 50;

        private void OnStartCombat(Unit unit, Unit enemy)
        {
            Debug.Log("On Start combat");
            if (enemy.IsCombatState())
                return;

            enemy.SetCombatState();
            //enemy.unitFoe = unit;
            unit.SetCombatState();
            DoCombatRound(unit, enemy, 1);
        }

        private void DoCombatRound(Unit unit, Unit enemy, float delay = 0)
        {
            LeanTween.delayedCall(delay, () =>
            {
                var enemyUnit = (EnemyUnit)enemy;
                // Make sure that the enemy is only fighting one aba at a time
                if (enemy.unitFoe != unit)
                {
                    unit.SetRoamingState();
                    var targetPos = unit.GetAbaTower().GetEdgePointWithinInfluence();
                    unit.SetDestination(targetPos);
                    return;
                }

                float percentage = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
                float winChance = abaWinChance;

                // if (enemy.hasCrystal)
                //     winChance -= 10;

                bool isWin = winChance < percentage;

                // Make Snrk2 always lose to enemy
                if (unit.unitType == UnitType.SNRK2)
                    isWin = false;

                if (isWin)
                {
                    // Enemy died before combat could be resolved
                    if (enemy == null)
                        return;

                    GameManager.Instance.UnregisterEnemy((EnemyUnit)enemy);
                    bool isEnemyAlive = enemy.TakeDamage(Util.gameSettings.upgradeSettings.abaUnitDamage);
                    LeanTween.scale(enemy.gameObject, Vector3.one * 1.1f, 0.15f).setLoopPingPong(1);

                    if (isEnemyAlive)
                    {
                        DoCombatRound(unit, enemy, 1); //enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
                    }
                    else
                    {
                        unit.SetRoamingState();
                        var targetPos = unit.GetAbaTower().GetEdgePointWithinInfluence();
                        unit.SetDestination(targetPos);
                    }
                }

                // If enemy won
                else
                {
                    // Enemy won but died before combat was over
                    if (enemy == null)
                    {
                        unit.SetRoamingState();
                        var targetPos = unit.GetAbaTower().GetEdgePointWithinInfluence();
                        unit.SetDestination(targetPos);
                        return;
                    }

                    if (!unit.isAlive)
                    {
                        enemy.StartMoving(enemyUnit.GetNextWaypoint(), 1.0f);
                        unit.KillUnit();

                        return;
                    }

                    bool isUnitAlive = unit.TakeDamage(Util.gameSettings.basicEnemyDamage);
                    LeanTween.scale(unit.gameObject, Vector3.one * 1.1f, 0.15f).setLoopPingPong(1);

                    if (isUnitAlive)
                    {
                        if (unit.unitType == UnitType.SNRK2)
                        {
                            enemy.StartMoving(enemyUnit.GetNextWaypoint(), 1.0f);
                            unit.SetRoamingState();
                        }
                        else
                        {
                            DoCombatRound(unit, enemy, 1);
                        }
                    }
                    else
                    {
                        enemy.StartMoving(enemyUnit.GetNextWaypoint(), 1.0f);
                        unit.KillUnit();
                    }

                    //LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(() => {

                    //});
                }
            });
        }

        private void OnEnable()
        {
            EventManager.Units.onStartCombat += OnStartCombat;
        }

        private void OnDisable()
        {
            EventManager.Units.onStartCombat -= OnStartCombat;
        }
    }
}