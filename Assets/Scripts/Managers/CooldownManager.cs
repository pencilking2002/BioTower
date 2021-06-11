using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using System;

namespace BioTower
{
public class CooldownManager : MonoBehaviour
{
    [SerializeField] private float structureSpawnCooldown = 5.0f;
    private float lastStructureSpawnedTime;
    public static Dictionary<StructureType,bool> structureCooldownMap = new Dictionary<StructureType, bool>();

    private void Awake()
    {
        CreateStructureCooldownMap();
    }

    private void CreateStructureCooldownMap()
    {
        foreach(StructureType structureType in Enum.GetValues(typeof(StructureType)))
        {
            structureCooldownMap.Add(structureType, true);
        }
    }

    private void OnStructureCreated(StructureType structureType)
    {
        structureCooldownMap[structureType] = false;
        EventManager.Structures.onStructureCooldownStarted?.Invoke(structureType, structureSpawnCooldown);
        LeanTween.delayedCall(structureSpawnCooldown, () => {
            structureCooldownMap[structureType] = true;
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