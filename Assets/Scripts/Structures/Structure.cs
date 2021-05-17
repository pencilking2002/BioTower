using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

namespace BioTower.Structures
{

public enum StructureState
{
    NONE,
    ACTIVE,
    DESTROYED
}

public enum StructureType
{
    ABA_TOWER,
    DNA_BASE,
    NONE,
    PPC2_TOWER
}

public class Structure : MonoBehaviour
{   
    [SerializeField] protected StructureType structureType;
    [SerializeField] private bool hasHealth;
    [EnableIf("hasHealth")] [Range(0,100)] [SerializeField] protected int maxHealth;
    [EnableIf("hasHealth")] [SerializeField] protected int currHealth;
    [EnableIf("hasHealth")] [SerializeField] protected Slider healthSlider;
    [SerializeField] StructureState structureState;
    [SerializeField] protected SpriteRenderer sr;

    public virtual void Awake()
    {
        currHealth = maxHealth;
        structureState = StructureState.ACTIVE;

        if (hasHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currHealth;
        }
    }

    public virtual void TakeDamage(int numDamage)
    {
        if (hasHealth)
        {
            currHealth -= numDamage;
            healthSlider.value = currHealth;
        }
    }

    public virtual void OnTapStructure()
    {
        
    }
}
}

