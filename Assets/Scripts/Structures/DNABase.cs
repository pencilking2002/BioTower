﻿using System.Collections;
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
    //public static Action onBaseDestroyed;
    private void LevelLoaded()
    {
        GameManager.Instance.RegisterPlayerBase(this);
    }

    private void OnBaseReached()
    {
        TakeDamage(1);
        if (currHealth > 0)
        {
            Util.ScaleBounceSprite(sr, 1.1f);
        }
        else
        {
            BaseDestroyed();
        }
    }

    private void BaseDestroyed()
    {
       
        EventManager.Structures.onBaseDestroyed?.Invoke();
    }

    private void OnEnable()
    {
        EventManager.Game.onLevelLoaded_01 += LevelLoaded;
        EventManager.Units.onEnemyBaseReached += OnBaseReached;
    }

    private void OnDisable()
    {
        EventManager.Game.onLevelLoaded_01 -= LevelLoaded;
        EventManager.Units.onEnemyBaseReached -= OnBaseReached;
    }
}
}

