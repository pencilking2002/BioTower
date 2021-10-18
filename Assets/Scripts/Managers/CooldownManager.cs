using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Structures;
using System;
using UnityEngine.UI;

namespace BioTower
{
public class CooldownManager : MonoBehaviour
{
    public float structureSpawnCooldown = 5.0f;
    private float lastStructureSpawnedTime;
    public static Dictionary<StructureType,bool> structureCooldownMap = new Dictionary<StructureType, bool>();
    
    //public float mitoLightFragCooldown = 3;

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

    // private void OnTapLightDropButton(Button button)
    // {
    //     button.interactable = false;
    //     var cooldownImage = button.transform.Find("Cooldown").GetComponent<Image>();
    //     LeanTween.value(gameObject, 1, 0, mitoLightFragCooldown).setOnUpdate((float val) => {
    //         cooldownImage.fillAmount = val;
    //     })
    //     .setOnComplete(() => {
    //         button.interactable = true;
    //     });
    // }
    private void OnEnable()
    {
        EventManager.Structures.onStructureCreated += OnStructureCreated;
        //EventManager.UI.onTapLightDropButton += OnTapLightDropButton;
    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
        //EventManager.UI.onTapLightDropButton -= OnTapLightDropButton;
    }
}
}