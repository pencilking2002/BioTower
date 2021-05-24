﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;

namespace BioTower
{
[CreateAssetMenu(fileName="GameSettings", menuName="GameSettings")]
public class GameSettings : ScriptableObject
{
    [Range(1,10)]public float timeScale = 1.0f;

    [Header("Damage")]
    public int ppc2Damage = 5;
    public int abaDamage = 5;
    public int basicEnemyDamage = 5;


    [Header("Unit Health")]
    public int abaUnitMaxHealth = 10;
    public int enemyUnitMaxHealth = 10;
    
    public int GetMaxUnitHealth(UnitType unitType)
    {
        switch(unitType)
        {
            case UnitType.ABA:
                return abaUnitMaxHealth;
            case UnitType.BASIC_ENEMY:
                return enemyUnitMaxHealth;
        }

        Debug.Log("Unit type not recognized: " + unitType);
        return 0;
    }
}
}
