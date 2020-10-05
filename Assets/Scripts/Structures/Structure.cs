using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace BioTower.Structures
{
public class Structure : MonoBehaviour
{   
    [SerializeField] private bool hasHealth;
    [EnableIf("hasHealth")] [SerializeField] private int maxHealth;
    [EnableIf("hasHealth")] [SerializeField] private int currHealth;

    public virtual void Awake()
    {
        currHealth = maxHealth;
    }
}
}

