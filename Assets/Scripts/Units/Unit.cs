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
        ABA,
        BASIC_ENEMY,
        SNRK2,
        MID_ENEMY,
        ADVANCED_ENEMY
    }

    public class Unit : MonoBehaviour
    {
        public UnitType unitType;
        [HideInInspector] public PolyNavAgent agent;
        [HideInInspector] public Structure tower;
        [SerializeField] private bool hasHealth;
        [EnableIf("hasHealth")][SerializeField] protected int currHealth;
        protected HealthBar healthBar;
        [HideInInspector] protected Animator anim;
        [HideInInspector] public SpriteRenderer sr;
        public bool isAlive;

        public virtual void Awake()
        {
            agent = GetComponent<PolyNavAgent>();
            anim = GetComponentInChildren<Animator>();
            sr = anim.GetComponent<SpriteRenderer>();
            healthBar = GetComponentInChildren<HealthBar>();
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
        }

        public ABATower GetAbaTower()
        {
            return (ABATower)tower;
        }

        public PPC2Tower GetPPC2Tower()
        {
            return (PPC2Tower)tower;
        }

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
                    EventManager.Units.onUnitTakeDamage?.Invoke(unitType);
            }
            else
            {
                KillUnit();
            }
            return isAlive;
        }

        public virtual void SetCombatState() { }
        public virtual void SetRoamingState() { }
        public virtual void SetDestroyedState() { }
        public virtual void SetDestination(Vector3 targetDestination) { }
        public virtual void Deregister() { }

        public virtual bool IsCombatState() { return false; }
        public virtual void KillUnit()
        {
            isAlive = false;
            EventManager.Units.onUnitDestroyed?.Invoke(this);
            GameManager.Instance.unitManager.Unregister(this);
            Destroy(gameObject);
        }

    }
}
