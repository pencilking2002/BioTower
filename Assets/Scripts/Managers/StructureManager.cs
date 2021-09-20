using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Structures
{
public class StructureManager : MonoBehaviour
{
    [SerializeField] private List<Structure> structureList = new List<Structure>();
    [SerializeField] private float declineDelay = 3;

    private void Update()
    {
        if (GameManager.Instance.gameStates.gameState != GameState.GAME)
            return;
            
        //(Structure structure in structureList)
        for(int i=0;i<structureList.Count; i++)
        {
            if (structureList[i] == null)
                continue;

            Structure structure = structureList[i];
            DoHealthDeclineOrHeal(structure);
            structure.OnUpdate();
        }
    }

    private void DoHealthDeclineOrHeal(Structure structure)
    {
        bool healEnabled = (structure.IsAbaTower() && Util.upgradeSettings.enableAbaTowerRandomHeal) ||
                           (structure.IsChloroTower() && Util.upgradeSettings.enableChloroTowerRandomHeal);

        if (Util.gameSettings.upgradeSettings.enableTowerHealthDecline)
        {
            if (Time.time > structure.lastDeclineTime + declineDelay)
            {
                structure.lastDeclineTime = Time.time;
                float healChance = GetRandomFloat();
                bool isHeal = healChance > 1-Util.gameSettings.randomHealChance && healEnabled;

                if (isHeal)
                    structure.GainHealth(Util.gameSettings.randomHealAmount);
                else
                    structure.TakeDamage(Util.gameSettings.declineDamage);
            }
        }
    }

    private float GetRandomFloat()
    {
        float healChance = 0;
        if (!Util.upgradeSettings.enableAbaTowerRandomHeal)
            return healChance;
        
        healChance = UnityEngine.Random.Range(0.0f,1.0f);
        return healChance; 

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