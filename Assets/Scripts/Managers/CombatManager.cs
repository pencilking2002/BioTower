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
            var unitScale = unit.transform.localScale;
            LeanTween.scale(unit.gameObject, unitScale * 1.2f, 0.25f).setLoopPingPong(6);
            unitScale = enemy.transform.localScale;

            LeanTween.scale(enemy.gameObject, unitScale * 1.2f, 0.25f)
                .setLoopPingPong(6)
                .setDelay(0.25f)
                .setOnComplete(() => {
                    ResolveCombat(unit, enemy);
                });
        }
        else if (unit.unitType == UnitType.SNRK2)
        {
            LeanTween.delayedCall(1.0f, () => {
                ResolveCombat(unit, enemy);
            });
        }
    }

    private void ResolveCombat(Unit unit, BasicEnemy enemy)
    {
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
            GameManager.Instance.UnregisterEnemy(enemy);
            bool isEnemyAlive = enemy.TakeDamage(GameManager.Instance.gameSettings.abaDamage);
            
            if (isEnemyAlive)
                enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
    
            unit.SetRoamingState();
            unit.SetNewDestination();
        }
        else
        {
           
            LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(() => {
                enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
                unit.Deregister();
                bool isUnitAlive = unit.TakeDamage(GameManager.Instance.gameSettings.basicEnemyDamage);

                if (isUnitAlive)
                {
                    unit.SetRoamingState();
                    unit.SetNewDestination();
                }
                else
                {
                    unit.SetDestroyedState();
                }
            });
        }
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