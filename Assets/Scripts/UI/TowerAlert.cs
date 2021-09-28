using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using BioTower.Structures;

namespace BioTower
{
public class TowerAlert : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool isAnimating;

    public void OnUpdate(Structure tower)
    {
        if (tower.structureType == StructureType.ABA_TOWER)
        {
            var numUnits = tower.units.Count;
            if (numUnits < Util.upgradeSettings.abaUnitSpawnLimit && !isAnimating)
            {
                StartAnimation();
            }
            else if (numUnits >= Util.upgradeSettings.abaUnitSpawnLimit && isAnimating)
            {
                StopAnimation();
            }
        }

    }

    private void StartAnimation()
    {
        isAnimating = true;
        anim.SetBool(Constants.isAnimating, true);
    }

    private void StopAnimation()
    {
        isAnimating = false;
        anim.SetBool(Constants.isAnimating, false);
    }
}
}