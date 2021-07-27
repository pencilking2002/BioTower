using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BioTower
{
public class EnemyCrystal : MonoBehaviour
{
    public bool hasBeenPickedUp;
    public bool isTargeted;
    
    private void Start()
    {
        EventManager.Game.onCrystalCreated?.Invoke(this);    
    }

    public void DestroyObject()
    {
        if (hasBeenPickedUp)
            return;
        
        EventManager.Game.onCrystalDestroyed?.Invoke(this);
        hasBeenPickedUp = true;

        GameManager.Instance.CreateCrystalExplosion(transform.position);
        LeanTween.scale(gameObject, Vector3.zero, 0.1f)
            .setOnComplete(() => {
                Destroy(gameObject);
            });
    }
}
}