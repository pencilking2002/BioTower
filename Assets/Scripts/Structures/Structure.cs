using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using BioTower.Units;

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
    CHLOROPLAST,
    MITOCHONDRIA,
    ROAD_BARRIER
}

public class Structure : MonoBehaviour
{   
    [SerializeField] private StructureSocket socket;
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
    public Vector3 initSpriteScale;
    [SerializeField] protected GameObject influenceVisuals;
    public List<Unit> units;
    public TowerAlert towerAlert;

    public virtual void Awake()
    {
        initSpriteScale = sr.transform.localScale;
        lastDeclineTime = Time.time;
    }

    // public virtual void Start()
    // {
        
    // }

    public virtual void Init(StructureSocket socket)
    {
        initScale = transform.localScale;
       
        currHealth = maxHealth;
        structureState = StructureState.ACTIVE;

        if (hasHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currHealth;
        }

        EventManager.Structures.onStructureCreated?.Invoke(this);

        if (structureType != StructureType.ROAD_BARRIER)
        {
            GameManager.Instance.tapManager.selectedStructure = this;
            GameManager.Instance.tapManager.hasSelectedStructure = true;
            this.socket = socket;
        }
        //Debug.Log("Init tower: " + structureType);
    }

    public virtual void OnUpdate()
    {
        
    }

    protected Unit GetClosestUnit(BasicEnemy enemy)
    {
        float furthestDistance = Mathf.Infinity;
        Unit closestUnit = null;

        foreach(var unit in units)
        {
            var distance = Vector2.Distance(unit.transform.position, enemy.transform.position);
            if (distance < furthestDistance)
            {
                furthestDistance = distance;
                closestUnit = unit;
            }
        }
        return closestUnit;
    }

    public virtual void TakeDamage(int numDamage)
    {
        if (GameManager.Instance.gameStates.IsGameState() && hasHealth && isAlive)
        {
            if (this == null)
                return;

            currHealth -= numDamage;
            currHealth = Mathf.Clamp(currHealth, 0, maxHealth);

            LeanTween.value(gameObject, healthSlider.value, currHealth, 0.25f)
            .setOnUpdate((float val) => {
                healthSlider.value = val;
            });
            EventManager.Structures.onStructureLoseHealth?.Invoke(this);

            if (currHealth == 0)
                KillStructure();
            else
                DoHealthVisual(new Color(1, 0.5f, 0.5f, 1), 1.01f);
        }
    }

    public virtual void GainHealth(int numHealth)
    {
        currHealth += numHealth;
        currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        healthSlider.value = currHealth;
        DoHealthVisual(Color.green);
        EventManager.Structures.onStructureGainHealth?.Invoke(this);
    }

    private void DoHealthVisual(Color targetColor, float scaleUpFactor=1.1f)
    {
        if (this == null)
            return;

        // Reset tower before applying tweens to it
        LeanTween.cancel(sr.gameObject);
        sr.transform.localScale = initSpriteScale;
        sr.color = Color.white;

        Util.ScaleBounceSprite(sr, scaleUpFactor);
        var oldColor = sr.color;
        sr.color = targetColor;
        LeanTween.value(sr.gameObject, sr.color, oldColor, 0.25f)
        .setOnUpdate((Color col) => {
            sr.color = col;
        });
    }

    public virtual int GetCurrHealth() { return currHealth; }
    public virtual int GetMaxHealth() { return maxHealth; }

    public virtual void KillStructure()
    {
        isAlive = false;
        GameManager.Instance.CreateTowerExplosion(transform.position);
        EventManager.Structures.onStructureDestroyed?.Invoke(this);

        if (socket != null)
            socket.SetHasStructure(false);
        
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

        if (structureType == StructureType.DNA_BASE && !Util.upgradeSettings.enablePlayerTowerHealing)
            return;

        Debug.Log($"Select {structure.gameObject.name}. This structure: {gameObject.name}. Same structure: {structure == this}");

        if (structure == this)
        {
            if (spriteOutline != null)
                spriteOutline.SetActive(true);

            if (influenceVisuals != null)
                influenceVisuals.SetActive(true);

            LeanTween.cancel(gameObject);
            transform.localScale = initScale;
            LeanTween.scale(gameObject, initScale * 1.1f, 0.1f).setLoopPingPong(1);
        }
        else
        {
            if (spriteOutline != null)
                spriteOutline.SetActive(false);

            if (influenceVisuals != null)
                influenceVisuals.SetActive(false);
        }
    }

    public bool IsAbaTower()
    {
        return structureType == StructureType.ABA_TOWER;
    }

    public bool IsPPC2Tower()
    {
        return structureType == StructureType.PPC2_TOWER;
    }
    
    public bool IsChloroTower()
    {
        return structureType == StructureType.CHLOROPLAST;
    }


    public int GetNumUnits()
    {
        return units.Count;
    }


    public virtual void OnStructureCreated(Structure structure)
    {
        OnStructureSelected(structure);
    }
    
    public virtual void OnEnable()
    {
        EventManager.Structures.onStructureSelected += OnStructureSelected;
        EventManager.Structures.onStructureCreated += OnStructureCreated;
    }

    public virtual void OnDisable()
    {
        EventManager.Structures.onStructureSelected -= OnStructureSelected;
        EventManager.Structures.onStructureCreated -= OnStructureCreated;
    }

    private void OnDestroy()
    {
        isAlive = false;
    }
}
}

