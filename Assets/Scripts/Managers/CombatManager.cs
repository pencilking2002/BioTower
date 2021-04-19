using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Units
{
public class CombatManager : MonoBehaviour
{
    [Range(0, 100)][SerializeField] private float abaWinChance = 50;

    private AbaUnit abaUnit;
    private BasicEnemy enemy;

    private void OnStartCombat(AbaUnit unit, BasicEnemy enemy)
    {
        unit.SetCombatState();
        enemy.StopMoving();
        unit.StopMoving();

        // Perform combat
        var unitScale = unit.transform.localScale;
        LeanTween.scale(unit.gameObject, unitScale * 2, 0.25f).setLoopPingPong(6);
        unitScale = enemy.transform.localScale;

        LeanTween.scale(enemy.gameObject, unitScale * 2, 0.25f)
            .setLoopPingPong(6)
            .setDelay(0.25f)
            .setOnComplete(() => {
                ResolveCombat(unit, enemy);
            });
    }

    private void ResolveCombat(AbaUnit unit, BasicEnemy enemy)
    {
        float percentage = UnityEngine.Random.Range(0.0f,1.0f) * 100;
        bool isWin = abaWinChance < percentage;

        if (isWin)
        {
            GameManager.Instance.UnregisterEnemy(enemy);
            enemy.KillUnit();
            unit.SetRoamingState();
            unit.SetNewDestination();
            Debug.Log("Aba unit win");
        }
        else
        {
            unit.SetDestroyedState();
            LeanTween.scale(gameObject, Vector3.zero, 0.2f).setOnComplete(() => {
                enemy.StartMoving(1.0f);
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