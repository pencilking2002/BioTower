using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BioTower.Structures;
using BioTower.Units;

namespace BioTower.UI
{
// TODO: Move this script to the GameCanvas GO os that it recieves events properly
public class NumUnitsController : MonoBehaviour
{
    [SerializeField] private Image[] unitCircles;

    private void SetCircles(int numActive, int total)
    {
        for (int i=0; i<unitCircles.Length;i++)
        {
            Image circle = unitCircles[i];
            bool isLessThanTotal = i<total;
            circle.enabled = isLessThanTotal;
            circle.color = i < numActive ? Color.green : Color.black;
        }
    }

    private void OnUnitSpawned(Unit unit)
    {
        if (unit.tower == null)
            return;

        var structure = unit.tower;

        if (structure.IsAbaTower())
            SetCircles(structure.units.Count, Util.upgradeSettings.abaUnitSpawnLimit);
        else if (structure.IsPPC2Tower())
            SetCircles(structure.units.Count, Util.upgradeSettings.ppc2UnitSpawnLimit);

        else
            SetCircles(0, 0);
        
    }

    private void OnStructureSelected(Structure structure)
    {
        if (structure.IsAbaTower())
            SetCircles(structure.units.Count, Util.upgradeSettings.abaUnitSpawnLimit);
        else if (structure.IsPPC2Tower())
            SetCircles(structure.units.Count, Util.upgradeSettings.ppc2UnitSpawnLimit);
        else
            SetCircles(0, 0);
    }

    private void OnStructureCreated(Structure structure)
    {
        Debug.Log("Created");
        
        if (structure.IsPPC2Tower())
            SetCircles(structure.units.Count, Util.upgradeSettings.ppc2UnitSpawnLimit);
        else
            SetCircles(0, 0);
    }

    private void OnEnable()
    {
        EventManager.Units.onUnitSpawned += OnUnitSpawned;
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.Structures.onStructureCreated += OnStructureCreated;
    }

    private void OnDisable()
    {
        EventManager.Units.onUnitSpawned -= OnUnitSpawned;
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
    }
}
}