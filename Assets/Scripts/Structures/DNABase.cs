using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using BioTower.Units;
using System;

namespace BioTower.Structures
{

[SelectionBase]
public class DNABase : Structure
{   
    //public static Action onBaseDestroyed;
    [SerializeField] private Sprite prestineStateSprite;
    [SerializeField] private Sprite hurtStateSprite;
    [SerializeField] private Sprite criticalStateSprite;
    [MinMaxSlider(0,100)][SerializeField] private Vector2 hurtMinRange;
    [MinMaxSlider(0,100)][SerializeField] private Vector2 criticalRange;

    private void LevelLoaded()
    {
        GameManager.Instance.RegisterPlayerBase(this);
    }

    public override void TakeDamage(int numDamage)
    {
        if (hasHealth && isAlive)
        {
            currHealth -= numDamage;
            healthSlider.value = currHealth;

            var healthPercentage = GetHealthPercentage();
            Debug.Log("Health percentage: " + healthPercentage);
            sr.sprite = prestineStateSprite;

            if (healthPercentage < hurtMinRange.y && healthPercentage > hurtMinRange.x)
                sr.sprite = hurtStateSprite;
            else if (healthPercentage <= criticalRange.y)
                sr.sprite = criticalStateSprite;

            if (currHealth <= 0)
                KillStructure();
        }
    }

    private float GetHealthPercentage()
    {
        return ((float) currHealth/ (float) maxHealth) * 100;
    }

    private void OnBaseReached()
    {
        TakeDamage(1);
        if (currHealth > 0)
        {
            Util.ScaleBounceSprite(sr, 1.1f);
        }
        else
        {
            BaseDestroyed();
        }
    }

    private void BaseDestroyed()
    {
       
        EventManager.Structures.onBaseDestroyed?.Invoke();
    }

    private void OnEnable()
    {
        EventManager.Game.onLevelLoaded_01 += LevelLoaded;
        EventManager.Units.onEnemyBaseReached += OnBaseReached;
    }

    private void OnDisable()
    {
        EventManager.Game.onLevelLoaded_01 -= LevelLoaded;
        EventManager.Units.onEnemyBaseReached -= OnBaseReached;
    }
}
}

