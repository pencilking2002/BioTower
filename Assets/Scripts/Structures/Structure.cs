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
    PPC2_TOWER,
    CHLOROPLAST
}

public class Structure : MonoBehaviour
{   
    public StructureType structureType;
    [SerializeField] public bool hasHealth;
    [EnableIf("hasHealth")] [SerializeField] protected bool isAlive = true;
    [EnableIf("hasHealth")] [Range(0,100)] [SerializeField] protected int maxHealth;
    [EnableIf("hasHealth")] [SerializeField] protected int currHealth;
    [EnableIf("hasHealth")] [SerializeField] protected Slider healthSlider;
    [SerializeField] protected GameObject spriteOutline;
    [SerializeField] StructureState structureState;
    public SpriteRenderer sr;
    [HideInInspector] public float lastDeclineTime;
    private Vector3 initScale;
    [SerializeField] protected GameObject influenceVisuals;

    public virtual void Awake()
    {
        initScale = transform.localScale;
        currHealth = maxHealth;
        structureState = StructureState.ACTIVE;

        if (hasHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currHealth;
        }
    }

    public virtual void Start()
    {
        EventManager.Structures.onStructureCreated?.Invoke(this);
        GameManager.Instance.tapManager.selectedStructure = this;
        GameManager.Instance.tapManager.hasSelectedStructure = true;
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void TakeDamage(int numDamage)
    {
        if (hasHealth && isAlive)
        {
            currHealth -= numDamage;

            LeanTween.value(gameObject, healthSlider.value, currHealth, 0.25f)
            .setOnUpdate((float val) => {
                healthSlider.value = val;
            });
            EventManager.Structures.onStructureLoseHealth?.Invoke(this);

            if (currHealth <= 0)
                KillStructure();
        }
    }

    public virtual void GainHealth(int numHealth)
    {
        currHealth += numHealth;
        healthSlider.value = currHealth;
        EventManager.Structures.onStructureGainHealth?.Invoke(this);
    }

    public virtual int GetCurrHealth() { return currHealth; }
    public virtual int GetMaxHealth() { return maxHealth; }

    public virtual void KillStructure()
    {
        isAlive = false;
        GameManager.Instance.CreateTowerExplosion(transform.position);
        EventManager.Structures.onStructureDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    public virtual void OnTapStructure(Vector3 screenPoint)
    {
        
    }

    public virtual void SpawnUnits(int numUnits)
    {

    }

    public virtual void OnStructureSelected(Structure structure)
    {
        if (structure == this)
        {
            spriteOutline?.SetActive(true);

            if (influenceVisuals != null)
                influenceVisuals.SetActive(true);

            LeanTween.cancel(gameObject);
            transform.localScale = initScale;
            //var oldScale = transform.localScale;
            LeanTween.scale(gameObject, initScale * 1.1f, 0.1f).setLoopPingPong(1);
        }
        else
        {
            spriteOutline?.SetActive(false);

            if (influenceVisuals != null)
                influenceVisuals?.SetActive(false);
        }
    }

    public virtual void OnStructureCreated(Structure structure)
    {
        OnStructureSelected(structure);
    }
    
    private void OnEnable()
    {
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.Structures.onStructureCreated += OnStructureCreated;

    }

    private void OnDisable()
    {
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
    }
}
}

