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
        if (!GameManager.Instance.gameSettings.enableTowerHealthDecline)
            return;
            
        foreach(Structure structure in structureList)
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
        structureList.Add(structure);
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