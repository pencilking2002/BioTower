using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BioTower.Units;
using Sirenix.OdinInspector;

namespace BioTower.Structures
{

public enum StructureState
{
    NONE, ACTIVE,
    DESTROYED
}

public enum StructureType
{
    ABA_TOWER, DNA_BASE,
    NONE, PPC2_TOWER,
    CHLOROPLAST, MITOCHONDRIA,
    ROAD_BARRIER, MINI_CHLOROPLAST_TOWER
}

public class Structure : MonoBehaviour
{   
    [SerializeField] protected StructureSocket socket;
    public StructureType structureType;
    [SerializeField] public bool hasHealth;
    [ShowIf("hasHealth")] public bool isAlive = true;
    [ShowIf("hasHealth")] [Range(0,100)] [SerializeField] protected int maxHealth;
    [ShowIf("hasHealth")] [SerializeField] protected int currHealth;
    [ShowIf("hasHealth")] [SerializeField] protected Slider healthSlider;
    [SerializeField] protected GameObject spriteOutline;
    public SpriteRenderer sr;
    [HideInInspector] public float lastDeclineTime;
    [SerializeField] protected GameObject influenceVisuals;
    public List<Unit> units;
    public TowerAlert towerAlert;
    private Vector3 initSpriteScale;
    private Vector3 initSpritePos;

    public virtual void Awake()
    {
        initSpriteScale = sr.transform.localScale;
        initSpritePos = sr.transform.position;
        lastDeclineTime = Time.time;
    }

    public virtual void Init(StructureSocket socket)
    {
        currHealth = maxHealth;

        if (hasHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currHealth;
        }

        EventManager.Structures.onStructureCreated?.Invoke(this);

        if (!IsBarrier() && !IsMiniChloroTower())
        {
            GameManager.Instance.tapManager.selectedStructure = this;
            GameManager.Instance.tapManager.hasSelectedStructure = true;
            this.socket = socket;
            AnimateTowerDrop();
        }
    }

    public virtual void AnimateTowerDrop()
    {
        var initPos = sr.transform.localPosition;
        var dropPosition = initPos + new Vector3(0, 25, 0);
        sr.transform.localPosition = dropPosition;

        var seq = LeanTween.sequence();
        
        seq.append(() => { 
            LeanTween.moveLocal(sr.gameObject, initPos, 0.3f);
            LeanTween.scale(sr.gameObject, initSpriteScale + new Vector3(0, 0.5f, 0), 0.3f);
        });

        seq.append(0.31f);
        var scale = initSpriteScale;
        scale.x *= 2f;
        scale.y *= 0.5f;
        if (socket != null)
            seq.append(socket.Jiggle);

        seq.append(LeanTween.scale(sr.gameObject, scale, 0.1f));
        seq.append(LeanTween.scale(sr.gameObject, initSpriteScale, 0.25f).setEaseOutExpo());
        

    }

    public virtual void OnUpdate() { }
    public virtual int GetCurrHealth() { return currHealth; }
    public virtual int GetMaxHealth() { return maxHealth; }

    public virtual bool IsMaxHealth() { return currHealth == maxHealth; }

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

    public virtual void KillStructure()
    {
        isAlive = false;
        GameManager.Instance.CreateTowerExplosion(transform.position);
        EventManager.Structures.onStructureDestroyed?.Invoke(this);

        if (socket != null)
            socket.SetHasStructure(false);
        
        Destroy(gameObject);
    }

    public virtual void OnTapStructure(Vector3 screenPoint) { }
    public virtual void SpawnUnits(int numUnits) { }

    public virtual void OnStructureSelected(Structure structure)
    {
        if (structure == null || GameManager.Instance == null)
            return;

        if (IsMiniChloroTower())
            return;
            
 //       Debug.Log($"Select {structure.gameObject.name}. This structure: {gameObject.name}. Same structure: {structure == this}");

        if (structure == this)
        {
            if (spriteOutline != null)
                spriteOutline.SetActive(true);

            if (influenceVisuals != null)
                influenceVisuals.SetActive(true);

            DoSquishyAnimation(initSpriteScale, initSpriteScale);
        }
        else
        {
            if (spriteOutline != null)
                spriteOutline.SetActive(false);

            if (influenceVisuals != null)
                influenceVisuals.SetActive(false);
        }
    }

    private void DoSquishyAnimation(Vector3 startingScale, Vector3 targetScale, bool cancelAnim=true)
    {
        if (cancelAnim)
            LeanTween.cancel(sr.gameObject);

        sr.transform.localScale = startingScale;
        float randScaleX = UnityEngine.Random.Range(1.2f, 1.4f);
        float randScaleY = UnityEngine.Random.Range(1.3f, 1.5f);
        var newScale = targetScale;
        newScale.x *= randScaleX;
        newScale.y *= randScaleY;

        var seq = LeanTween.sequence();
        seq.append(LeanTween.scale(sr.gameObject, newScale, 0.1f));
        seq.append(LeanTween.scale(sr.gameObject, targetScale, 0.2f));
        //Debug.Log("Do squishy anim");
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

    public bool IsMitoTower()
    {
        return structureType == StructureType.MITOCHONDRIA;
    }
    
    public bool IsMiniChloroTower()
    {
        return structureType == StructureType.MINI_CHLOROPLAST_TOWER;
    }

    public bool IsBarrier()
    {
        return structureType == StructureType.ROAD_BARRIER;
    }

    public bool IsPlayerBase()
    {
        return structureType == StructureType.DNA_BASE;
    }

    public int GetNumUnits()
    {
        return units.Count;
    }

    public virtual void OnStructureCreated(Structure structure)
    {
        //OnStructureSelected(structure);
        Debug.Log($"{structureType} created");
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

    private void OnDestroy() { isAlive = false; }
}
}

