using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Structures
{
public class StructureManager : MonoBehaviour
{
    [SerializeField] private List<Structure> structureList = new List<Structure>();
    [SerializeField] private float declineDelay = 3;
    [SerializeField] private int declineDamage = 1;

    private void Update()
    {
        //(Structure structure in structureList)
        for(int i=0;i<structureList.Count; i++)
        {
            Structure structure = structureList[i];
            DoHealthDecline(structure);
            structure.OnUpdate();
        }
    }

    private void DoHealthDecline(Structure structure)
    {
        if (GameManager.Instance.gameSettings.enableTowerHealthDecline)
        {
            if (Time.time > structure.lastDeclineTime + declineDelay)
            {
                structure.TakeDamage(declineDamage);
                structure.lastDeclineTime = Time.time;
            }
        }
    }

    private void OnStructureCreated(Structure structure)
    {
        if (structure.structureType == StructureType.DNA_BASE)
            return;

        structureList.Add(structure);
    }

    private void OnStructureDestroyed(Structure structure)
    {
        structureList.Remove(structure);
    }

    private void OnEnable()
    {
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        EventManager.Structures.onStructureDestroyed += OnStructureDestroyed;

    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        EventManager.Structures.onStructureDestroyed -= OnStructureDestroyed;
    }
}
}