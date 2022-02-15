using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using System;
using BioTower.Level;
using BioTower.Structures;

namespace BioTower.Units
{
    [SelectionBase]
    public class BasicEnemy : Unit
    {

        [Header("References")]
        [SerializeField] private GameObject crystalPrefab;
        [SerializeField] private Collider2D triggerCollider;
        [SerializeField] private SpriteRenderer muscleIcon;
        [SerializeField] private SpriteRenderer upgradedSprite;
        [SerializeField] private SpriteRenderer currentSR;
        [SerializeField] private Animator currentAnim;


        [Header("Enemy state")]
        [SerializeField] private Color stoppedColor;
        public bool hasCrystal;
        public Color hasCrystalTintColor;
        private bool isRegistered;
        public bool isEngagedInCombat;
        public Unit combatFoe;

        [Header("Waypoint movement")]
        [SerializeField] private Waypoint currWaypoint;
        [SerializeField] private Waypoint nextWaypoint;
        [SerializeField] private Transform nextDestination;


        public override void Start()
        {
            base.Start();
            GameManager.Instance.RegisterEnemy(this);
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

        public override void StopMoving()
        {
            if (agent == null)
                return;

            agent.Stop();
            isEngagedInCombat = true;
            currentAnim.SetBool("Walk", false);
            currentAnim.SetBool("Attack", true);
            Debug.Log("Enemy stop moving");
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

        public override void StartMoving(Waypoint waypoint, float delay = 0)
        {
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

        private void SpawnCrystal()
        {
            var crystalGO = Instantiate(crystalPrefab);
            crystalGO.transform.position = transform.position;

            Vector3 defautlScale = crystalGO.transform.localScale;
            crystalGO.transform.localScale = Vector3.zero;
            LeanTween.scale(crystalGO, defautlScale, 0.1f);
        }

        public override bool TakeDamage(int amount)
        {
            if (base.TakeDamage(amount))
            {
                return isAlive;
            }
            else
            {
                SpawnCrystal();
                triggerCollider.enabled = false;
                return isAlive;
            }
        }

        private void AnimateMuscleIcon()
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

        private void PickupCrystal(Collider2D col)
        {
            var crystal = col.transform.parent.GetComponent<EnemyCrystal>();

            if (crystal.hasBeenPickedUp)
                return;

            crystal.DestroyObject();
            hasCrystal = true;
            sr.enabled = false;
            sr.GetComponent<Animator>().enabled = false;
            sr = upgradedSprite;
            upgradedSprite.gameObject.SetActive(true);
            currentSR = upgradedSprite;
            currentAnim = upgradedSprite.GetComponent<Animator>();

            EventManager.Units.onEnemyPickedUpCrystal?.Invoke();

            AnimateMuscleIcon();
            // TODO: make enemy stronger after picking up crystal

        }

        private void RegisterWithTower(Collider2D col)
        {
            var tower = col.transform.parent.GetComponent<Structure>();
            if (tower.structureType == StructureType.ABA_TOWER)
            {
                var abaTower = (ABATower)tower;
                abaTower.RegisterEnemy(this);
            }
        }

        private void UnregisterWithTower(Collider2D col)
        {
            var tower = col.transform.parent.GetComponent<Structure>();
            if (tower.structureType == StructureType.ABA_TOWER)
            {
                var abaTower = (ABATower)tower;
                abaTower.UnregisterEnemy(this);
            }
        }

        private void HandleBarrierCollision(Collider2D col)
        {
            int instanceID = col.transform.parent.gameObject.GetInstanceID();
            EventManager.Units.onEnemyBarrierCollision?.Invoke(instanceID);
            TakeDamage(1000);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == 13)
                PickupCrystal(col);

            if (col.gameObject.layer == 19)
                RegisterWithTower(col);

            if (col.gameObject.layer == 20)
                HandleBarrierCollision(col);
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.layer == 19)
                UnregisterWithTower(col);
        }

        private void DestinationReached()
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
                //            Debug.Log("Base reached");
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

        // private void OnTogglePaths()
        // {
        //     LeanTween.delayedCall(0.1f, () => {
        //         Vector3 targetPos = GameManager.Instance.playerBase.transform.position;
        //         agent.SetDestination(targetPos);
        //     });
        //     Debug.Log("ENEMY TOGGLE PATHS");
        // }

        private void OnGameStateInit(GameState gameState)
        {
            if (gameState == GameState.GAME_OVER_LOSE || gameState == GameState.GAME_OVER_WIN)
            {
                agent.Stop();
                currentAnim.SetBool("Walk", false);
                currentAnim.SetBool("Attack", false);
                currentAnim.speed = 0;
            }
        }

        private void OnEnable()
        {
            agent.OnDestinationReached += DestinationReached;
            //EventManager.Game.onTogglePaths += OnTogglePaths;
            EventManager.Game.onGameStateInit += OnGameStateInit;
        }

        private void OnDisable()
        {
            Debug.Log("ENEMY DISABLE");
            agent.OnDestinationReached -= DestinationReached;
            //EventManager.Game.onTogglePaths -= OnTogglePaths;
            EventManager.Game.onGameStateInit -= OnGameStateInit;
        }

    }
}
