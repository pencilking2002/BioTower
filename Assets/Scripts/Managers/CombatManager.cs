using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower.Units
{
    public class CombatManager : MonoBehaviour
    {
        [Range(0, 100)][SerializeField] private float abaWinChance = 50;
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
            if (!unit.IsRoamingState())
                return;

            // Find the closest enemy who is not engaged
            var enemy = tower.FindClosestRoamingEnemy(unit, out bool isFound);

            if (!isFound)
                return;

            unit.SetChasingState(enemy);
            enemy.SetChasingState(unit);
        }

        private void SetUnitCombatStates(ABATower tower)
        {
            foreach (Unit unit in tower.units)
            {
                if (!unit.IsChasingState())
                    return;

                float distance = Vector2.Distance(unit.transform.position, unit.unitFoe.transform.position);
                if (distance < combatDistanceThreshold)
                {
                    unit.SetCombatState();
                    unit.unitFoe.SetCombatState();
                }
            }
        }
    }
}