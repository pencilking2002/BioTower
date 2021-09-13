using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Units
{
public class CombatManager : MonoBehaviour
{
    [Range(0, 100)][SerializeField] private float abaWinChance = 50;
    [Range(0, 100)][SerializeField] private float enemyCrystalWinChanceBoost = 10;
    private AbaUnit abaUnit;
    private BasicEnemy enemy;

    private void OnStartCombat(Unit unit, BasicEnemy enemy)
    {
        unit.SetCombatState();
        enemy.StopMoving();
        unit.StopMoving();

        // Perform combat

        if (unit.unitType == UnitType.ABA)
        {
           
            // var unitScale = unit.transform.localScale;
            // LeanTween.scale(unit.gameObject, unitScale * 1.2f, 0.25f).setLoopPingPong(6);
            // unitScale = enemy.transform.localScale;

            // LeanTween.scale(enemy.gameObject, unitScale * 1.2f, 0.25f)
            //     .setLoopPingPong(6)
            //     .setDelay(0.25f)
            //     .setOnComplete(() => {
            //         ResolveCombat(unit, enemy);
            //     });
        }
        else if (unit.unitType == UnitType.SNRK2)
        {
            // LeanTween.delayedCall(1.0f, () => {
            //     ResolveCombat(unit, enemy);
            // });
        }
        DoCombatRound(unit, enemy, 1);
    }

    private void DoCombatRound(Unit unit, BasicEnemy enemy, float delay=0)
    {
        LeanTween.delayedCall(delay, () => {
            float percentage = UnityEngine.Random.Range(0.0f,1.0f) * 100;
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
                bool isEnemyAlive = enemy.TakeDamage(Util.gameSettings.upgradeSettings.abaDamage);
                LeanTween.scale(enemy.gameObject, Vector3.one * 1.1f, 0.15f).setLoopPingPong(1);

                if (isEnemyAlive)
                {
                    DoCombatRound(unit, enemy, 1); //enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
                }
                else
                {
                    unit.SetRoamingState();
                    unit.SetNewDestination();
                }
            }

            // If enemy won
            else
            {
                // Enemy won but died before combat was over
                if (enemy == null)
                {
                    unit.SetRoamingState();
                    unit.SetNewDestination();
                    return;
                }

                if (!unit.isAlive)
                {
                    enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
                    unit.Deregister();
                    unit.SetDestroyedState();

                    return;
                }
            
                bool isUnitAlive = unit.TakeDamage(Util.gameSettings.upgradeSettings.basicEnemyDamage);
                LeanTween.scale(unit.gameObject, Vector3.one * 1.1f, 0.15f).setLoopPingPong(1);
                
                if (isUnitAlive)
                {
                    DoCombatRound(unit, enemy, 1);
                    // unit.SetRoamingState();
                    // unit.SetNewDestination();
                }
                else
                {
                    enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
                    unit.Deregister();
                    unit.SetDestroyedState();
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