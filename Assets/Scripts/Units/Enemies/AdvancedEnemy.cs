using UnityEngine;

namespace BioTower.Units
{
    [SelectionBase]
    public class AdvancedEnemy : EnemyUnit
    {
        public override void Start()
        {
            Init();
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

        public override void DestinationReached()
        {
            base.DestinationReached();
        }

        public override void OnGameStateInit(GameState gameState)
        {
            base.OnGameStateInit(gameState);
        }

        private void OnEnable()
        {
            agent.OnDestinationReached += DestinationReached;
            EventManager.Game.onGameStateInit += OnGameStateInit;
        }

        private void OnDisable()
        {
            agent.OnDestinationReached -= DestinationReached;
            EventManager.Game.onGameStateInit -= OnGameStateInit;
        }

    }
}
