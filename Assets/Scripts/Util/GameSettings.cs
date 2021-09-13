using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using Sirenix.OdinInspector;
using BioTower.Structures;

namespace BioTower
{
[CreateAssetMenu(fileName="GameSettings", menuName="GameSettings")]
public class GameSettings : ScriptableObject
{
    [Range(1,10)]public float timeScale = 1.0f;

    [Header("Units")]
    [TabGroup("Units")][ShowInInspector][ReadOnly] public static int ppc2Damage = 5;
    [TabGroup("Units")][ShowInInspector][ReadOnly] public static int abaDamage = 5;
    [TabGroup("Units")][ShowInInspector][ReadOnly] public static int basicEnemyDamage = 5;
    [TabGroup("Units")][ShowInInspector][ReadOnly] public static int abaUnitMaxHealth = 10;
    [TabGroup("Units")][ShowInInspector][ReadOnly] public static int snrkUnitMaxHealth = 5;
    [TabGroup("Units")][ShowInInspector][ReadOnly] public static int enemyUnitMaxHealth = 10;
    [TabGroup("Units")][ShowInInspector][ReadOnly] public static int abaUnitCost = 5;
    [TabGroup("Units")][ShowInInspector][ReadOnly] public static int snark2UnitCost = 1;


    [Header("Tower Health")]
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static bool enableTowerHealthDecline = false;


    [Header("Tower Cost")]
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static int ppc2TowerCost = 40;
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static int chloroplastTowerCost = 20;
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static int mitoTowerCost = 20;
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static int healTowerCost = 5;
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static int spawnLightDropCost = 1;
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static int healTowerAmount = 2;

    [Header("ABA Tower")]
    [TabGroup("Towers")][Range(0,100)][ShowInInspector][ReadOnly] public static int abaTowerCost = 10;
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static int abaUnitSpawnLimit = 3;
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static float abaMaxInfluenceRadius = 2.31f;
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static float abaMapScale = 2.31f;
    [TabGroup("Towers")][ShowInInspector][ReadOnly] public static float abaInfluenceShapeRadius = 2.3f;


    [TabGroup("Misc")][ShowInInspector][ReadOnly] public static int startingLevel = 1;
    [TabGroup("Misc")][ShowInInspector][ReadOnly] public static int lightFragmentValue = 5;
    [TabGroup("Misc")][ShowInInspector][ReadOnly] public static int crystalValue = 5;
    [TabGroup("Misc")][ShowInInspector][ReadOnly] public static int crystalSnrk2Value = 20;
    [TabGroup("Misc")][ShowInInspector][ReadOnly] public static int startingEnergy = 80;
    //[TabGroup("Misc")][ShowInInspector][ReadOnly] public static int energy;

    // public void Reset()
    // {
    //     timeScale = 1;

    //     ppc2Damage = 5;
    //     abaDamage = 5;
    //     basicEnemyDamage = 5;
    //     abaUnitMaxHealth = 10;
    //     snrkUnitMaxHealth = 5;
    //     enemyUnitMaxHealth = 10;
    //     abaUnitCost = 5;
    //     snark2UnitCost = 1;

    //     enableTowerHealthDecline = false;
    //     ppc2TowerCost = 40;
    //     chloroplastTowerCost = 20;
    //     mitoTowerCost = 20;
    //     healTowerCost = 5;
    //     spawnLightDropCost = 1;
    //     healTowerAmount = 2;
    //     abaTowerCost = 10;
    //     abaUnitSpawnLimit = 3;
    //     abaMaxInfluenceRadius = 2.31f;
    //     abaMapScale = 2.31f;
    //     abaInfluenceShapeRadius = 2.3f;

    //     startingLevel = 1;
    //     lightFragmentValue = 5;
    //     crystalValue = 5;
    //     crystalSnrk2Value = 20;
    //     startingEnergy = 80;
    //     energy = 80;
    // }
}
}
