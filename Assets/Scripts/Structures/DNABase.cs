using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using BioTower.Units;
using System;

namespace BioTower.Structures
{

[SelectionBase]
public class DNABase : Structure
{   
    public static Action onBaseDestroyed;
    private void LevelLoaded()
    {
        GameManager.Instance.RegisterPlayerBase(this);
    }

    private void OnBaseReached()
    {
        TakeDamage(1);
        BaseDestroyed();
    }

    private void BaseDestroyed()
    {
        if (currHealth <= 0)
            onBaseDestroyed?.Invoke();
    }

    private void OnEnable()
    {
        GameManager.onLevelLoaded_01 += LevelLoaded;
        BasicEnemy.onBaseReached += OnBaseReached;
    }

    private void OnDisable()
    {
        GameManager.onLevelLoaded_01 -= LevelLoaded;
        BasicEnemy.onBaseReached -= OnBaseReached;
    }
}
}

