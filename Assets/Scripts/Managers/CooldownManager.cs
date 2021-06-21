using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using System;

namespace BioTower
{
public class CooldownManager : MonoBehaviour
{
    public float structureSpawnCooldown = 5.0f;
    private float lastStructureSpawnedTime;
    public static Dictionary<StructureType,bool> structureCooldownMap = new Dictionary<StructureType, bool>();

    private void Start()
    {
        structureCooldownMap = new Dictionary<StructureType, bool>();
        CreateStructureCooldownMap();
    }

    private void CreateStructureCooldownMap()
    {
        foreach(StructureType structureType in Enum.GetValues(typeof(StructureType)))
        {
            structureCooldownMap.Add(structureType, true);
        }
    }

    private void OnStructureCreated(Structure structure)
    {
        structureCooldownMap[structure.structureType] = false;
        EventManager.Structures.onStructureCooldownStarted?.Invoke(structure.structureType, structureSpawnCooldown);
        LeanTween.delayedCall(structureSpawnCooldown, () => {
            structureCooldownMap[structure.structureType] = true;
        }); 
    }

    private void OnEnable()
    {
        EventManager.Structures.onStructureCreated += OnStructureCreated;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
    }
}
}