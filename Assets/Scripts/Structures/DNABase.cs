using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using BioTower.Units;

namespace BioTower.Structures
{

[SelectionBase]
public class DNABase : Structure
{   
    private void LevelLoaded()
    {
        GameManager.Instance.RegisterPlayerBase(this);
    }

    private void OnBaseReached()
    {
        TakeDamage(1);
    }

    private void OnEnable()
    {
        GameManager.onLevelLoaded_01 += LevelLoaded;
        BasicEnemy.onBaseReached += OnBaseReached;
    }

    private void OnDisable()
    {
        GameManager.onLevelLoaded_01 -= LevelLoaded;
        BasicEnemy.onBaseReached += OnBaseReached;
    }
}
}

