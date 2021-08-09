using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;

namespace BioTower
{
public class ParticleManager : MonoBehaviour
{
    public GameObject snrk2ExplosionPrefab;
    public GameObject abaExplosionPrefab;

    private void OnUnitDestroyed(Unit unit)
    {
        if (unit.unitType == UnitType.SNRK2)
        {
            var explosion = Instantiate(snrk2ExplosionPrefab);
            explosion.transform.position = transform.position;
            LeanTween.delayedCall(gameObject, 0.5f, () => {
                Destroy(explosion);
            });
        }
        else if (unit.unitType == UnitType.ABA)
        {
            var explosion = Instantiate(abaExplosionPrefab);
            explosion.transform.position = transform.position;
            LeanTween.delayedCall(gameObject, 0.5f, () => {
                Destroy(explosion);
            });
        }
    }

    private void OnEnable()
    {
        EventManager.Units.onUnitDestroyed += OnUnitDestroyed;
    }

    private void OnDisable()
    {
        EventManager.Units.onUnitDestroyed -= OnUnitDestroyed;
    }
}
}