using UnityEngine;
using BioTower.Level;
using BioTower.Structures;

namespace BioTower.Units
{
    public class EnemyUnit : Unit
    {
        [Header("References")]
        //[SerializeField] protected GameObject crystalPrefab;
        protected Collider2D triggerCollider;
        [SerializeField] protected SpriteRenderer muscleIcon;
        // [SerializeField] protected SpriteRenderer upgradedSprite;
        // protected SpriteRenderer currentSR;
        protected Animator currentAnim;


        //[Header("Waypoint movement")]
        protected Waypoint currWaypoint;
        protected Waypoint nextWaypoint;
        protected Transform nextDestination;


        [Header("Enemy state")]
        // [SerializeField] protected Color stoppedColor;
        // public Color hasCrystalTintColor;
        [HideInInspector] public bool hasCrystal;
        //[HideInInspector] private bool isRegistered;
        [HideInInspector] public bool isEngagedInCombat;
        [HideInInspector] public Unit combatFoe;

        protected void Init()
        {
            base.Start();
            triggerCollider = GetComponentInChildren<Collider2D>();
            currentAnim = anim;
            GameManager.Instance.RegisterEnemy(this);
        }

        protected void RegisterWithTower(Collider2D col)
        {
            var tower = col.transform.parent.GetComponent<Structure>();
            if (tower.structureType == StructureType.ABA_TOWER)
            {
                var abaTower = (ABATower)tower;
                abaTower.RegisterEnemy(this);
            }
        }

        protected void UnregisterWithTower(Collider2D col)
        {
            var tower = col.transform.parent.GetComponent<Structure>();
            if (tower.structureType == StructureType.ABA_TOWER)
            {
                var abaTower = (ABATower)tower;
                abaTower.UnregisterEnemy(this);
            }
        }

        protected void HandleBarrierCollision(Collider2D col)
        {
            int instanceID = col.transform.parent.gameObject.GetInstanceID();
            EventManager.Units.onEnemyBarrierCollision?.Invoke(instanceID);
            TakeDamage(1000);
        }

        public virtual void DestinationReached()
        {
            SetCurrWaypoint(nextWaypoint);

            if (currWaypoint.isFork)
            {
                var nextPoint = currWaypoint.ChooseNextWaypoint();
                SetNextWaypoint(nextPoint);
                SetDestination(nextPoint);
            }
            else if (currWaypoint.isEndpoint)
            {
                EventManager.Units.onEnemyBaseReached?.Invoke(this);
                KillUnit();
            }
            else
            {
                var nextPoint = currWaypoint.nextWaypoint;
                SetNextWaypoint(nextPoint);
                SetDestination(nextPoint);
            }
            //Debug.Log($"{gameObject.name} reached {currWaypoint.gameObject.name}");

            EventManager.Units.onEnemyReachedDestination?.Invoke(this);
        }

        public virtual void OnGameStateInit(GameState gameState)
        {
            if (gameState == GameState.GAME_OVER_LOSE || gameState == GameState.GAME_OVER_WIN)
            {
                agent.Stop();
                currentAnim.SetBool("Walk", false);
                currentAnim.SetBool("Attack", false);
                currentAnim.speed = 0;
            }
        }

        /// <summary>
        /// Sets the current waypoint, its last waypoint the enemy encountered
        /// </summary>
        /// <param name="waypoint"></param>
        public void SetCurrWaypoint(Waypoint waypoint) { currWaypoint = waypoint; }

        /// <summary>
        /// Sets the next waypoint, its where the enemy is going
        /// </summary>
        /// <param name="waypoint"></param>
        public void SetNextWaypoint(Waypoint waypoint) { nextWaypoint = waypoint; }
        public Waypoint GetCurrWaypoint() { return currWaypoint; }
        public Waypoint GetNextWaypoint() { return nextWaypoint; }

        public void SetSpeed(Vector2 minMaxSpeed, float duration = 0)
        {
            float targetSpeed = UnityEngine.Random.Range(minMaxSpeed.x, minMaxSpeed.y);
            if (Mathf.Approximately(duration, 0))
            {
                agent.maxSpeed = targetSpeed;
            }
            else
            {
                LeanTween.value(gameObject, agent.maxSpeed, targetSpeed, 0.5f).setOnUpdate((float val) =>
                {
                    agent.maxSpeed = val;
                });
            }
        }

        public override void StartMoving(Waypoint waypoint, float delay = 0)
        {
            if (currentAnim == null)
                currentAnim = anim;

            currentAnim.SetBool("Walk", true);
            currentAnim.SetBool("Attack", false);
            combatFoe = null;
            LeanTween.delayedCall(delay, () =>
            {
                SetDestination(waypoint);
                isEngagedInCombat = false;
                Debug.Log("Enemy start moving");
            });
        }

        public override void StopMoving()
        {
            if (agent == null)
                return;

            agent.Stop();
            isEngagedInCombat = true;
            currentAnim.SetBool("Walk", false);
            currentAnim.SetBool("Attack", true);
            //Debug.Log("Enemy stop moving");
        }

        public void SetDestination(Waypoint waypoint)
        {
            if (agent == null)
                return;

            var randomPoint = UnityEngine.Random.insideUnitSphere * 0.25f;
            randomPoint.z = 0;
            //agent.SetDestination(waypoint.transform.position + randomPoint);
            agent.SetDestination(waypoint.transform.position);
            nextDestination = waypoint.transform;
            //Debug.Log($"{gameObject.name} Set destination to {nextDestination}");
        }

        // protected void PickupCrystal(Collider2D col)
        // {
        //     var crystal = col.transform.parent.GetComponent<EnemyCrystal>();

        //     if (crystal.hasBeenPickedUp)
        //         return;

        //     crystal.DestroyObject();
        //     hasCrystal = true;
        //     sr.enabled = false;
        //     sr.GetComponent<Animator>().enabled = false;
        //     sr = upgradedSprite;
        //     upgradedSprite.gameObject.SetActive(true);
        //     currentSR = upgradedSprite;
        //     currentAnim = upgradedSprite.GetComponent<Animator>();

        //     EventManager.Units.onEnemyPickedUpCrystal?.Invoke();

        //     AnimateMuscleIcon();
        //     // TODO: make enemy stronger after picking up crystal

        // }

        protected void AnimateMuscleIcon()
        {
            muscleIcon.gameObject.SetActive(true);
            var targetScale = muscleIcon.transform.localScale;
            muscleIcon.transform.localScale = Vector3.zero;
            LeanTween.scale(muscleIcon.gameObject, targetScale, 0.25f).setEaseOutElastic();

            LeanTween.delayedCall(gameObject, 1.0f, () =>
            {
                LeanTween.scale(muscleIcon.gameObject, Vector3.zero, 0.25f);
            });
        }

        // protected void SpawnCrystal()
        // {
        //     var crystalGO = Instantiate(crystalPrefab);
        //     crystalGO.transform.position = transform.position;

        //     Vector3 defautlScale = crystalGO.transform.localScale;
        //     crystalGO.transform.localScale = Vector3.zero;
        //     LeanTween.scale(crystalGO, defautlScale, 0.1f);
        // }

        public override bool TakeDamage(int amount)
        {
            if (base.TakeDamage(amount))
            {
                return isAlive;
            }
            else
            {
                //SpawnCrystal();
                triggerCollider.enabled = false;
                return isAlive;
            }
        }

    }
}
