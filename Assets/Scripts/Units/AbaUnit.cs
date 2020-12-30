using UnityEngine;
using PolyNav;

namespace BioTower.Units
{
[SelectionBase]
public class AbaUnit : Unit
{
    public PolyNavAgent agent;
    
    private void Awake()
    {
        //agent.enabled = false;
    }

    public override void Start()
    {
        base.Start();
    }
}
}
