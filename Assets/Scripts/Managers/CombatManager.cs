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

    private void OnStartCombat(AbaUnit unit, BasicEnemy enemy)
    {
        unit.SetCombatState();
        enemy.StopMoving();
        unit.StopMoving();

        // Perform combat
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

    private void ResolveCombat(AbaUnit unit, BasicEnemy enemy)
    {
        float percentage = UnityEngine.Random.Range(0.0f,1.0f) * 100;
        float winChance = abaWinChance;

        if (enemy.hasCrystal)
            winChance -= 10;
        
        bool isWin = winChance < percentage;
        
        if (isWin)
        {
            GameManager.Instance.UnregisterEnemy(enemy);
            enemy.KillUnit();
            unit.SetRoamingState();
            unit.SetNewDestination();
        }
        else
        {
            unit.SetDestroyedState();
            LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(() => {
                enemy.StartMoving(enemy.GetNextWaypoint(), 1.0f);
                unit.abaTower.RemoveUnit(unit);
                unit.KillUnit();
            });
        }
    }

    private void OnEnable()
    {
        AbaUnit.onStartCombat += OnStartCombat;
    }

    private void OnDisable()
    {
        AbaUnit.onStartCombat -= OnStartCombat;
    }
}
}