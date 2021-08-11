﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using BioTower.Level;
using UnityEngine.UI;
using PolyNav;

namespace BioTower.Units
{
public enum UnitType
{
    ABA,
    BASIC_ENEMY,
    SNRK2
}

public class Unit : MonoBehaviour
{
    public UnitType unitType;
    public PolyNavAgent agent;
    [SerializeField] private bool hasHealth;
    //[EnableIf("hasHealth")] [Range(0,100)] [SerializeField] private int maxHealth;
    [EnableIf("hasHealth")] [SerializeField] private int currHealth;
    [EnableIf("hasHealth")] [SerializeField] private Slider healthSlider;
    public SpriteRenderer sr;
    public bool isAlive;
    
    public virtual void Start()
    {
        if (hasHealth)
        {
            currHealth = GameManager.Instance.gameSettings.GetMaxUnitHealth(unitType);
            healthSlider.maxValue = currHealth;
            healthSlider.value = currHealth;
            healthSlider.gameObject.SetActive(true);
        }
        else 
        {
            if (healthSlider != null)
                healthSlider.gameObject.SetActive(false);
        }
        isAlive = true;

        GameManager.Instance.unitManager.Register(this);
        EventManager.Units.onUnitSpawned?.Invoke(this);
    }

    public virtual void StopMoving() { }
    public virtual void StartMoving(Waypoint waypoint, float delay=0) { }

    /// <summary>
    /// Unit takes damage
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>whether the unit is alive after taking damage</returns>
    public virtual bool TakeDamage(int amount)
    {
        if (hasHealth)
        {
            currHealth -= amount;
            currHealth = Mathf.Clamp(currHealth, 0, GameManager.Instance.gameSettings.GetMaxUnitHealth(unitType));
            healthSlider.value = currHealth;

//            Debug.Log("Enemy take damage. health: " + currHealth);
            if (currHealth == 0)
            {
                KillUnit();
            }
        }
        else
        {
            KillUnit();
        }
        return isAlive;
    }

    public virtual void SetCombatState() {  } 
    public virtual void SetRoamingState() {  }
    public virtual void SetDestroyedState() { }
    public virtual void SetNewDestination() { }
    public virtual void Deregister() { }

    protected void KillUnit() 
    { 
        isAlive = false;
        EventManager.Units.onUnitDestroyed?.Invoke(this);
        GameManager.Instance.unitManager.Unregister(this); 
        Destroy(gameObject);
    }
    
}
}
