using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using BioTower.SaveData;

namespace BioTower
{
public enum UpgradeType
{
    // ABA
    ABA_TOWER_INFLUENCE, ABA_TOWER_UNIT_COST,
    ABA_TOWER_RANDOM_HEAL, ABA_UNIT_HEALTH, 
    ABA_UNIT_DAMAGE, ABA_UNIT_SPEED, 
    ABA_UNIT_SPAWN_LIMIT,
    
    // PPC2
    PP2C_TOWER_UNLOCK, PP2C_TOWER_INFLUENCE,
    PP2C_TOWER_FIRE_RATE, PP2C_EXPLOSION_RADIUS,
    PP2C_TOWER_DAMAGE, PP2C_TOWER_RANDOM_HEAL,

    // MITO
    MITO_TOWER_UNLOCK, MITO_TOWER_ENERGY,
    MITO_TOWER_COOLDOWN, MITO_TOWER_HEALTH,

    // CHOLOROPLAST
    CHLORO_TOWER_UNLOCK, CHLORO_TOWER_ENERGY,
    CHLORO_TOWER_ENERGY_RATE, CHLORO_TOWER_LIGHT_CLICK,
    CHLORO_TOWER_RANDOM_HEAL,

    // SNRK2
    SNRK2_UNIT_UNLOCK, SNRK2_SPEED,
    SNRK2_DAMAGE_RESISTANCE, SNRK2_CRYSTAL_VALUE,

    // Player base
    PLAYER_TOWER_HEALTH, PLAYER_TOWER_HEAL
}

[CreateAssetMenu(fileName="UpgradeTree", menuName="UpgradeTree")]
public class UpgradeTree : ScriptableObject
{
    public Upgrade[] upgradeTree;
}

[Serializable]
public class Upgrade
{
    [BoxGroup] public bool isUnlock;
    [BoxGroup] public LevelType level;
    [BoxGroup] [ShowIf("isUnlock")] public UpgradeType unlockUpgrade;
    [BoxGroup] [HideIf("isUnlock")] public UpgradeType upgrade_01;
    [BoxGroup] [HideIf("isUnlock")] public UpgradeType upgrade_02;
    [BoxGroup] [HideIf("isUnlock")] public UpgradeType upgrade_03;
}
}