using UnityEngine;

namespace BioTower.Units
{



    [SelectionBase]
    public class AbaUnit : Unit
    {

        [Header("References")]
        public Rigidbody rb;

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            Util.objectShake.Shake(GameManager.Instance.cam.gameObject, 0.2f, 0.02f);
            base.Start();
            Util.ScaleUpSprite(sr, 1.1f);

            var targetPos = GetAbaTower().GetEdgePointWithinInfluence();
            SetDestination(targetPos);

            agent.maxSpeed = Util.upgradeSettings.abaUnitMaxSpeed_float.GetFloat();

        }

        public override void StopMoving()
        {
            if (!agent)
                return;

            agent.Stop();
            agent.enabled = false;
            anim.SetBool("Walk", false);
        }

        public override bool IsRoamingState()
        {
            return base.IsRoamingState();
        }

        public override void SetRoamingState()
        {
            var targetPos = GetAbaTower().GetEdgePointWithinInfluence();
            SetDestination(targetPos);
            base.SetRoamingState();
        }

        public override bool IsChasingState()
        {
            return base.IsChasingState();
        }

        public override void SetChasingState(Unit unit)
        {
            base.SetChasingState(unit);
        }

        public override bool IsCombatState()
        {
            return base.IsCombatState();
        }

        public override void SetCombatState()
        {
            base.SetCombatState();
            StopMoving();
            anim.SetBool("Attack", true);
        }

        public override void SetDestroyedState()
        {
            if (!isAlive || !gameObject)
                return;

            isAlive = false;
            base.SetDestroyedState();
            StopMoving();
            GameManager.Instance.unitManager.Unregister(this);
            Deregister();

            if (healthBar)
                healthBar.gameObject.SetActive(false);

            if (explosion)
                explosion.Play();

            Destroy(gameObject);
        }

        public override void SetDestination(Vector3 newDestination)
        {
            if (agent == null)
                return;

            agent.enabled = true;
            if (agent == null)
                return;

            agent.SetDestination(newDestination);
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);
        }

        private void OnDestinationReached()
        {
            if (GameManager.Instance.gameStates.gameState != GameState.GAME)
                return;

            if (IsRoamingState())
            {
                var targetPos = GetAbaTower().GetEdgePointWithinInfluence();
                SetDestination(targetPos);
            }
        }

        public override void Deregister()
        {
            GetAbaTower().RemoveUnit(this);
        }

        public override void KillUnit()
        {
            SetDestroyedState();
            EventManager.Units.onUnitDestroyed?.Invoke(this);
        }

        private void OnGameStateInit(GameState gameState)
        {
            if (gameState == GameState.GAME_OVER_LOSE || gameState == GameState.GAME_OVER_WIN)
            {
                agent.Stop();
                anim.SetBool("Walk", false);
                anim.SetBool("Attack", false);
            }
        }

        private void OnEnable()
        {
            agent.OnDestinationReached += OnDestinationReached;
            agent.OnDestinationInvalid += OnDestinationReached; // Used for when the destination is inside am obstacle
            EventManager.Game.onGameStateInit += OnGameStateInit;
        }

        private void OnDisable()
        {
            agent.OnDestinationReached -= OnDestinationReached;
            agent.OnDestinationInvalid -= OnDestinationReached;
            EventManager.Game.onGameStateInit -= OnGameStateInit;
        }
    }
}
