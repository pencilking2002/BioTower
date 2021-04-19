using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BioTower.Units
{
public class Unit : MonoBehaviour
{
    [SerializeField] private bool hasHealth;
    [EnableIf("hasHealth")] [Range(0,100)] [SerializeField] private int maxHealth;
    [EnableIf("hasHealth")] [SerializeField] private int currHealth;

    public virtual void Start()
    {
        currHealth = maxHealth;
    }

    public virtual void StopMoving() { }
    public virtual void StartMoving(float delay=0) { }

    public virtual void KillUnit() { Destroy(gameObject); }
}
}
