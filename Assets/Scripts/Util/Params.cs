﻿using UnityEngine;
using Sirenix.OdinInspector;
using BioTower.Structures;
using BioTower.Units;
using System;

namespace BioTower
{
    [Serializable]
    public class Params
    {
        [TabGroup("Misc")] public int currLevel = 1;
        [TabGroup("Misc")] public int lightFragmentValue = 5;
        [TabGroup("Misc")] public int energy = 80;


        [Header("ABA Units")]
        [TabGroup("Units")] public int abaUnitDamage = 5;           // Done
        [TabGroup("Units")] public int abaUnitMaxSpeed_float = 500; // Done
        [TabGroup("Units")] public int abaUnitCost = 3;             // Done
        [TabGroup("Units")] public int abaUnitMaxHealth = 10;       // Done


        [Header("SNRK2 Units")]
        [TabGroup("Units")] public int snrkUnitMaxHealth = 5;       // Done
        [TabGroup("Units")] public bool snrk2UnitUnlocked = false;  // Done
        [TabGroup("Units")] public int snrk2UnitSpeed_float = 1000; // Done
        [TabGroup("Misc")] public int crystalSnrk2Value = 20;


        [Header("Tower Misc")]
        [TabGroup("Towers")] public int healTowerCost = 5;                      // Done
        [TabGroup("Towers")] public bool enableTowerHealthDecline = false;      // Done
        [TabGroup("Towers")] public int playerTowerMaxhealth = 20;              // Done
        [TabGroup("Towers")] public bool enablePlayerTowerHealing = false;      // Done


        [Header("ABA Tower")]
        [TabGroup("Towers")][Range(0, 100)] public int abaTowerCost = 30;       // Done
        [TabGroup("Towers")] public int abaUnitSpawnLimit = 3;                 // Done
        [TabGroup("Towers")] public int abaMaxInfluenceRadius_float = 2600;    // Done
        [TabGroup("Towers")] public bool enableAbaTowerRandomHeal = false;     // Done


        [Header("PPC2 Tower")]
        [TabGroup("Towers")] public bool ppc2TowerUnlocked = false;             // Done
        [TabGroup("Towers")] public int ppc2TowerDamage = 5;         // Done
        [TabGroup("Towers")][Range(0, 100)] public int ppc2TowerCost = 60;                     // Done
        [TabGroup("Towers")] public int ppc2MaxInfluenceRadius_float = 2400;    // Done
        [TabGroup("Towers")] public int ppc2shootInterval_float = 1000;             // Done
        [TabGroup("Towers")] public int ppc2ExplosionColliderScale_float = 1271;   // Done
        [TabGroup("Towers")] public int ppc2ExplosionSpriteScale_float = 622;       // Done
        [TabGroup("Towers")] public int ppc2UnitSpawnLimit = 2;                 // Done


        [Header("Choloplast Tower")]
        [TabGroup("Towers")] public bool chloroTowerUnlocked = false;           // Done
        [TabGroup("Towers")] public int chloroplastTowerCost = 20;              // Done
        [TabGroup("Towers")] public int chloroShootInterval_float = 5000;       // Done
        [TabGroup("Towers")] public bool enableChloroTowerRandomHeal = false;   // Done


        [Header("Mitochondira Tower")]
        [TabGroup("Towers")] public int mitoTowerCost = 20;                     // Done
        [TabGroup("Towers")] public bool mitoTowerUnlocked = false;             // Done
        [TabGroup("Towers")] public int mitoShootInterval_float = 2000;         // Done
        [TabGroup("Towers")] public int mitoTowerMaxHealth = 10;                // Done 
        [TabGroup("Towers")] public int numFragmentsPickedUpOnTap = 1;          // Done


        public int GetTowerCost(StructureType structureType)
        {
            int cost = 0;
            switch (structureType)
            {
                case StructureType.ABA_TOWER:
                    cost = abaTowerCost;
                    break;
                case StructureType.PPC2_TOWER:
                    cost = ppc2TowerCost;
                    break;
                case StructureType.CHLOROPLAST:
                    cost = chloroplastTowerCost;
                    break;
                case StructureType.MITOCHONDRIA:
                    cost = mitoTowerCost;
                    break;
                default:
                    Debug.Log("Unable to find structure type:" + structureType.ToString());
                    break;
            }
            return cost;
        }
    }

    public static class ExtensionMethods
    {
        public static float GetFloat(this int inputInt)
        {
            return (float)inputInt / 1000.0f;
        }
    }
}