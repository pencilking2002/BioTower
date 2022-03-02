﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Units
{
    public class CombatManager : MonoBehaviour
    {
        [Range(0, 100)][SerializeField] private float abaWinChance = 50;
        //[Range(0, 100)][SerializeField] private float enemyCrystalWinChanceBoost = 10;
        //private AbaUnit abaUnit;
        //private BasicEnemy enemy;

        private void OnStartCombat(Unit unit, EnemyUnit enemy)
        {
            if (enemy.isEngagedInCombat || enemy.combatFoe != null)
                return;

            enemy.StopMoving();
            enemy.combatFoe = unit;
            enemy.isEngagedInCombat = true;
            unit.StopMoving();
            unit.SetCombatState();
            DoCombatRound(unit, enemy, 1);
            Debug.Log("On Start combat");
        }

        private void DoCombatRound(Unit unit, EnemyUnit enemy, float delay = 0)
        {
            LeanTween.delayedCall(delay, () =>
            {

                // Make sure that the enemy is only fighting one aba at a time
                if (enemy.combatFoe != unit)
                {
                    unit.SetRoamingState();
                    var targetPos = unit.GetAbaTower().GetEdgePointWithinInfluence();
                    unit.SetDestination(targetPos);
                    return;
                }

                float percentage = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
                float winChance = abaWinChance;

                if (enemy.hasCrystal)
                    winChance -= 10;

                bool isWin = winChance < percentage;

                // Make Snrk2 always lose to enemy
                if (unit.unitType == UnitType.SNRK2)
                    isWin = false;

                if (isWin)
                {
                    // Enemy died before combat could be resolved
                    if (enemy == null)
                        return;

                    GameManager.Instance.UnregisterEnemy(enemy);
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
                        enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
                        unit.KillUnit();

                        return;
                    }

                    bool isUnitAlive = unit.TakeDamage(Util.gameSettings.basicEnemyDamage);
                    LeanTween.scale(unit.gameObject, Vector3.one * 1.1f, 0.15f).setLoopPingPong(1);

                    if (isUnitAlive)
                    {
                        if (unit.unitType == UnitType.SNRK2)
                        {
                            enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
                            unit.SetRoamingState();
                        }
                        else
                        {
                            DoCombatRound(unit, enemy, 1);
                        }
                    }
                    else
                    {
                        enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
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