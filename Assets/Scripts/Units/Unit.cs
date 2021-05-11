using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using BioTower.Level;

namespace BioTower.Units
{
public class Unit : MonoBehaviour
{
    [SerializeField] private bool hasHealth;
    [EnableIf("hasHealth")] [Range(0,100)] [SerializeField] private int maxHealth;
    [EnableIf("hasHealth")] [SerializeField] private int currHealth;
    [SerializeField] protected SpriteRenderer sr;
    
    public virtual void Start()
    {
        currHealth = maxHealth;
    }

    public virtual void StopMoving() { }
    public virtual void StartMoving(Waypoint waypoint, float delay=0) { }

    public virtual void KillUnit() { Destroy(gameObject); }
}
}
