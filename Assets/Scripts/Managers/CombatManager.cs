using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;

namespace BioTower.Units
{
    public class CombatManager : MonoBehaviour
    {
        [Range(0, 100)][SerializeField] private float abaWinChance = 50;

        private void Update()
        {
            SetUnitChasingStates();
        }

        private void SetUnitChasingStates()
        {
            foreach (Structure tower in Util.structureManager.structureList)
            {
                if (!tower.IsAbaTower())
                    continue;

                var abaTower = (ABATower)tower;
                foreach (Unit unit in tower.units)
                {
                    if (!unit.IsRoamingState())
                        continue;

                    // Find the closest enemy who is not engaged
                    var enemy = abaTower.FindClosestRoamingEnemy(unit, out bool isFound);

                    if (!isFound)
                        continue;

                    unit.SetChasingState(enemy);
                    enemy.SetChasingState(unit);
                }
            }
        }
    }
}