using UnityEngine;
using BioTower.Level;
using BioTower.Structures;

namespace BioTower.Units
{
    public class EnemyUnit : Unit
    {
        [Header("References")]
        protected Collider2D triggerCollider;
        [SerializeField] protected SpriteRenderer muscleIcon;
        protected Animator currentAnim;


        protected Waypoint currWaypoint;
        protected Waypoint nextWaypoint;
        protected Transform nextDestination;


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
            if (!IsRoamingState())
                return;

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
            unitFoe = null;
            LeanTween.delayedCall(delay, () =>
            {
                SetDestination(waypoint);
                SetRoamingState();
                //Debug.Log("Enemy start moving");
            });
        }

        public override void StopMoving()
        {
            if (agent == null)
                return;

            agent.Stop();
            currentAnim.SetBool("Walk", false);
            currentAnim.SetBool("Attack", true);
        }

        public override void SetDestination(Waypoint waypoint)
        {
            if (agent == null)
                return;

            var randomPoint = UnityEngine.Random.insideUnitSphere * 0.25f;
            randomPoint.z = 0;
            agent.SetDestination(waypoint.transform.position);
            nextDestination = waypoint.transform;
        }

        public override void SetDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
            //agent.primeGoal = destination;
        }

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

        public override bool TakeDamage(int amount)
        {
            if (base.TakeDamage(amount))
            {
                return isAlive;
            }
            else
            {
                triggerCollider.enabled = false;
                return isAlive;
            }
        }
        public override bool IsRoamingState()
        {
            return base.IsRoamingState();
        }

        public override void SetRoamingState()
        {
            agent.map = GameManager.Instance.levelMap.map;
            agent.stoppingDistance = 1;
            agent.slowingDistance = 1;
            DestinationReached();
            base.SetRoamingState();
        }

        public override bool IsChasingState()
        {
            return base.IsChasingState();
        }

        public override bool IsCombatState()
        {
            return base.IsCombatState();
        }

        public override void SetChasingState(Unit unit)
        {
            if (unit.IsAba())
            {
                var abaTower = (ABATower)unit.tower;
                agent.map = abaTower.map;
            }
            agent.stoppingDistance = 0.1f;
            agent.slowingDistance = 0.1f;
            base.SetChasingState(unit);
        }

        public override void SetCombatState()
        {
            base.SetCombatState();
            StopMoving();
        }

    }
}
