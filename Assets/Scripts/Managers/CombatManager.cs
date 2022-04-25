using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower.Units
{
    public class CombatManager : MonoBehaviour
    {
        // [Range(0, 100)][SerializeField] private float abaWinChance = 50;
        // [Range(0, 100)][SerializeField] private float snrk2WinChance = 0;
        [SerializeField] private float combatDistanceThreshold = 0.25f;
        private void Update()
        {
            foreach (Structure tower in Util.structureManager.structureList)
            {
                if (!tower.IsAbaTower())
                    continue;

                var abaTower = (ABATower)tower;

                SetUnitChasingStates(abaTower);
                SetUnitCombatStates(abaTower);
            }
        }

        private void SetUnitChasingStates(ABATower tower)
        {
            foreach (Unit unit in tower.units)
            {
                ProcessAbaUnit(unit, tower);
            }
        }

        private void ProcessAbaUnit(Unit unit, ABATower tower)
        {
            if (unit.IsRoamingState())
            {
                // Find the closest enemy who is not engaged
                var enemy = tower.FindClosestRoamingEnemy(unit, out bool isFound);

                if (!isFound)
                    return;

                unit.SetChasingState(enemy);
                enemy.SetChasingState(unit);
            }
        }

        private void SetUnitCombatStates(ABATower tower)
        {
            foreach (Unit unit in tower.units)
            {
                if (unit.IsChasingState())
                {
                    float distance = Vector2.Distance(unit.transform.position, unit.unitFoe.transform.position);
                    if (distance < combatDistanceThreshold)
                    {
                        unit.SetCombatState();
                        unit.unitFoe.SetCombatState();
                        DoCombatRound(unit, unit.unitFoe, 1);
                    }
                }
            }
        }

        private void DoCombatRound(Unit unit, Unit enemyUnit, float delay = 0)
        {
            LeanTween.delayedCall(delay, () =>
            {
                float percentage = UnityEngine.Random.Range(0.0f, 1.0f) * 100;
                float winChance = GetWinChance(unit);

                bool isWin = winChance > percentage;
                //Debug.Log($"UnitType: {unit.unitType}. IsWin: {isWin}. Win chance: {winChance}. Percentage: {percentage}");

                if (isWin)
                {
                    HandleUnitWin(unit, enemyUnit);
                }
                else
                {
                    HandleUnitWin(enemyUnit, unit);
                }
            });
        }

        private float GetWinChance(Unit unit)
        {
            float winChance = 0;
            if (unit.IsAba())
                winChance = Util.gameSettings.abaWinChance;
            else if (unit.IsSnrk2())
                winChance = Util.gameSettings.snrk2WinChance;

            return winChance;
        }

        private void HandleUnitWin(Unit winningUnit, Unit losingUnit)
        {
            int damage = GetDamage(winningUnit);

            bool isUnitAlive = losingUnit.TakeDamage(damage);
            if (isUnitAlive)
                DoCombatRound(winningUnit, losingUnit, 1);
            else
                winningUnit.SetRoamingState();
        }

        private int GetDamage(Unit unit)
        {
            int damage = 0;
            if (unit.IsAba())
                damage = Util.gameSettings.upgradeSettings.abaUnitDamage;
            else if (unit.IsEnemy())
                damage = Util.gameSettings.basicEnemyDamage;

            return damage;
        }
    }
}