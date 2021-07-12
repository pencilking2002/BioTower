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
    [SerializeField] protected bool hasHealth;
    [EnableIf("hasHealth")] [SerializeField] protected bool isAlive = true;
    [EnableIf("hasHealth")] [Range(0,100)] [SerializeField] protected int maxHealth;
    [EnableIf("hasHealth")] [SerializeField] protected int currHealth;
    [EnableIf("hasHealth")] [SerializeField] protected Slider healthSlider;
    [SerializeField] protected GameObject spriteOutline;
    [SerializeField] StructureState structureState;
    public SpriteRenderer sr;
    [HideInInspector] public float lastDeclineTime;

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

    public virtual void Start()
    {
        EventManager.Structures.onStructureCreated?.Invoke(this);

        //if (GameManager.Instance != null)
        //{
            GameManager.Instance.tapManager.selectedStructure = this;
            GameManager.Instance.tapManager.hasSelectedStructure = true;
        //}
    }

    public virtual void OnUpdate()
    {
        
    }

    public virtual void TakeDamage(int numDamage)
    {
        if (hasHealth && isAlive)
        {
            currHealth -= numDamage;
            healthSlider.value = currHealth;

            if (currHealth <= 0)
                KillStructure();
        }
    }

    public virtual void GainHealth(int numHealth)
    {
        currHealth += numHealth;
        healthSlider.value = currHealth;
    }

    public virtual void KillStructure()
    {
        isAlive = false;
        GameManager.Instance.CreateTowerExplosion(transform.position);
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
        if (spriteOutline != null)
            spriteOutline.SetActive(structure == this);
        
        if (structure == this)
        {
            var oldScale = transform.localScale;
            LeanTween.scale(gameObject, oldScale * 1.1f, 0.1f).setLoopPingPong(1);
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

