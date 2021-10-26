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
    [SerializeField] private Color hurtColor;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Init(null);
        
        maxHealth = Util.upgradeSettings.playerTowerMaxhealth;
        currHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

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
//            Debug.Log("Health percentage: " + healthPercentage);
            sr.sprite = prestineStateSprite;

            if (healthPercentage < hurtMinRange.y && healthPercentage > hurtMinRange.x)
                sr.sprite = hurtStateSprite;
            else if (healthPercentage <= criticalRange.y)
                sr.sprite = criticalStateSprite;

            if (currHealth <= 0)
            {
                KillStructure();
                EventManager.Structures.onBaseDestroyed?.Invoke();
            }
            else
            {
                EventManager.Structures.onBaseTakeDamage?.Invoke();
            }
        }
    }

    private float GetHealthPercentage()
    {
        return ((float) currHealth/ (float) maxHealth) * 100;
    }

    private void OnBaseReached()
    {
        if (currHealth > 0)
        {
            LeanTween.cancel(gameObject);
            LeanTween.cancel(sr.gameObject);
            Util.ScaleBounceSprite(sr, 1.1f);
            var oldColor = Color.white;
            sr.color = hurtColor;
            LeanTween.value(gameObject, sr.color, oldColor, 0.25f).setOnUpdate((Color col) => {
                sr.color = col;
            });
        }
        TakeDamage(Util.gameSettings.enemyPlayerBaseDamage);
        Debug.Log("cancel");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        EventManager.Game.onLevelLoaded_01 += LevelLoaded;
        EventManager.Units.onEnemyBaseReached += OnBaseReached;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        EventManager.Game.onLevelLoaded_01 -= LevelLoaded;
        EventManager.Units.onEnemyBaseReached -= OnBaseReached;
    }
}
}

