using UnityEngine;
using NaughtyAttributes;
using BioTower.Level;
using UnityEngine.UI;
using PolyNav;
using BioTower.Structures;
using BioTower.UI;

namespace BioTower.Units
{
    public enum UnitType
    {
        ABA, BASIC_ENEMY,
        SNRK2, MID_ENEMY,
        ADVANCED_ENEMY
    }
    public enum UnitState
    {
        ROAMING, COMBAT,
        DESTROYED, CHASING_UNIT
    }

    public class Unit : MonoBehaviour
    {
        public UnitType unitType;
        public UnitState unitState;
        public Unit unitFoe;
        public ParticleExplosion explosion;

        [HideInInspector] public PolyNavAgent agent;
        [HideInInspector] public Structure tower;
        [SerializeField] private bool hasHealth;
        [EnableIf("hasHealth")][SerializeField] protected int currHealth;
        [SerializeField] protected HealthBar healthBar;
        [HideInInspector] protected Animator anim;
        [HideInInspector] public SpriteRenderer sr;
        public bool isAlive;

        public virtual void Awake()
        {
            agent = GetComponent<PolyNavAgent>();
            anim = GetComponentInChildren<Animator>();
            sr = anim.GetComponent<SpriteRenderer>();
            healthBar = GetComponentInChildren<HealthBar>();
            unitState = UnitState.ROAMING;

        }

        public virtual void Start()
        {
            if (hasHealth)
            {
                healthBar.Init(currHealth);
                healthBar.gameObject.SetActive(true);
            }
            else
            {
                if (healthBar != null)
                    healthBar.gameObject.SetActive(false);
            }
            isAlive = true;

            GameManager.Instance.unitManager.Register(this);
            EventManager.Units.onUnitSpawned?.Invoke(this);
            SetRoamingState();
        }

        public ABATower GetAbaTower()
        {
            return (ABATower)tower;
        }

        public PPC2Tower GetPPC2Tower()
        {
            return (PPC2Tower)tower;
        }
        // public virtual void StartChasingUnit(Unit unitFoe)
        // {
        //     SetChasingState();
        //     this.unitFoe = unitFoe;
        // }

        public virtual void StopMoving() { }
        public virtual void StartMoving(Waypoint waypoint, float delay = 0) { }

        /// <summary>
        /// Unit takes damage
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>whether the unit is alive after taking damage</returns>
        public virtual bool TakeDamage(int amount)
        {
            if (hasHealth)
            {
                currHealth -= amount;
                currHealth = Mathf.Clamp(currHealth, 0, 100);
                healthBar.SetHealth(currHealth);

                if (currHealth == 0)
                    KillUnit();
                else
                {
                    sr.color = GameManager.Instance.util.hurtColor;
                    float duration = 0.1f;
                    LeanTween.delayedCall(duration, () =>
                    {
                        if (sr) sr.color = Color.white;
                    });
                    var scale = sr.transform.localScale;
                    LeanTween.scale(sr.gameObject, scale * 1.5f, duration).setLoopPingPong(1);
                    EventManager.Units.onUnitTakeDamage?.Invoke(unitType);
                }
            }
            else
            {
                KillUnit();
            }
            return isAlive;
        }

        public bool IsEnemy()
        {
            bool isEnemy = unitType == UnitType.BASIC_ENEMY || unitType == UnitType.MID_ENEMY || unitType == UnitType.ADVANCED_ENEMY;
            return isEnemy;
        }

        public bool IsAba() { return unitType == UnitType.ABA; }
        public bool IsSnrk2() { return unitType == UnitType.SNRK2; }

        public virtual bool IsChasingState() { return unitState == UnitState.CHASING_UNIT; }
        public virtual void SetChasingState(Unit unit)
        {
            unitState = UnitState.CHASING_UNIT;
            unitFoe = unit;
            SetDestination(unit.transform.position);
        }

        public virtual bool IsRoamingState() { return unitState == UnitState.ROAMING; }
        public virtual void SetCombatState() { unitState = UnitState.COMBAT; }
        public virtual void SetRoamingState()
        {
            unitState = UnitState.ROAMING;
            unitFoe = null;
            //Debug.Log("Set roaming state");
        }

        public virtual void SetDestroyedState() { unitState = UnitState.DESTROYED; }
        public virtual void SetDestination(Waypoint waypoint) { }
        public virtual void SetDestination(Vector3 destination) { }
        public virtual void Deregister() { }

        public virtual bool IsCombatState() { return unitState == UnitState.COMBAT; }
        public virtual void KillUnit()
        {
            //Debug.Log("Kill unit");
            if (explosion)
                explosion.Play();

            isAlive = false;
            EventManager.Units.onUnitDestroyed?.Invoke(this);
            GameManager.Instance.unitManager.Unregister(this);
            Destroy(gameObject);
        }


    }
}
