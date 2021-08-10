using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower.Units
{
public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<Unit> units;

    public void Register(Unit unit)
    {
        units.Add(unit);
    }

    public void Unregister(Unit unit)
    {
        units.Remove(unit);
    }

    private void Update()
    {
        if (!GameManager.Instance.gameStates.IsGameState())
            return;
        
        for (int i=0; i<units.Count; i++)
        {
            Unit unit = units[i];
            Vector2 moveDir = unit.agent.movingDirection;

            if (moveDir.sqrMagnitude > 0.1f)
            {
                if (unit != null && unit.sr != null)
                {
                    float dot = Vector2.Dot(moveDir.normalized, Vector2.right) * 0.5f + 0.5f;
                    bool faceRight = dot > 0.5f;
                    unit.sr.flipX = faceRight;
                }
            }
        }
    }

}
}