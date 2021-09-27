using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Structures
{
public class RoadBarrier : Structure
{
    public override void Awake()
    {
        base.Awake();
        Init(null);
    }

    private void OnEnemyBarrierCollision(int instanceID)
    {
        if (gameObject.GetInstanceID() == instanceID)
        {
            TakeDamage(2);
            Debug.Log("Barrier Take damage");
        }
    }

    public override void OnEnable()
    {
        EventManager.Units.onEnemyBarrierCollision += OnEnemyBarrierCollision;
    }

    public override void OnDisable()
    {
        EventManager.Units.onEnemyBarrierCollision -= OnEnemyBarrierCollision;
    }
}
}