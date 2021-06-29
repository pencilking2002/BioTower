using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public abstract class BootStateBase : MonoBehaviour
{
    public GameState gameState;
    protected BootController controller;
    [SerializeField] protected bool isInitialized;

    public virtual void Awake() 
    { 
        controller = GetComponentInParent<BootController>();
    }

    public virtual void Init(GameState gameState) { }
    public abstract GameState OnUpdate(GameState gameState);

    public abstract void OnGameStateInit(GameState gameState);

  
}   
}