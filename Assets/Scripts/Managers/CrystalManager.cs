using UnityEngine;
using System.Collections.Generic;

namespace BioTower
{
public class CrystalManager : MonoBehaviour
{
    public List<EnemyCrystal> crystals;

    public bool HasValidCrystals()
    {
        foreach(EnemyCrystal crystal in crystals)
        {
            if (!crystal.isTargeted)
                return true;
        }
        return false;
    }

    public EnemyCrystal FindValidCrystal(Transform unitTransform)
    {
        EnemyCrystal c = null;
        float closestDistance = 1000000;
        foreach(EnemyCrystal crystal in crystals)
        {
            if (!crystal.isTargeted)
            {
                var distance = Vector2.Distance(unitTransform.position, crystal.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    c = crystal;
                }
            }
        }
        return c;
    }

    private void OnCrystalCreated(EnemyCrystal crystal)
    {
        if (!crystals.Contains(crystal))
            crystals.Add(crystal);
    }

    private void OnCrystalDestroyed(EnemyCrystal crystal)
    {
        if (crystals.Contains(crystal))
            crystals.Remove(crystal);
    }

    private void OnEnable()
    {
        EventManager.Game.onCrystalCreated += OnCrystalCreated;
        EventManager.Game.onCrystalDestroyed += OnCrystalDestroyed;
    }

    private void OnDisable()
    {
        EventManager.Game.onCrystalCreated -= OnCrystalCreated;
        EventManager.Game.onCrystalDestroyed -= OnCrystalDestroyed;
    }
}
}