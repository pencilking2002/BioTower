using UnityEngine;
using PolyNav;
using BioTower.Structures;

namespace BioTower.Units
{

public enum AbaUnitState
{
    ROAMING,
    CARRYING_ENEMY
}

[SelectionBase]
public class AbaUnit : Unit
{
    public AbaUnitState abaUnitState;
    public Vector2 destinationPoint;

    [Header("Roaming Settings")]
    public float roamingDuration = 2.0f;
    [HideInInspector] public bool hasTargetRoamingPoint;


    [Header("Carrying enemy state")]
    [SerializeField] private BasicEnemy carriedEnemy;
    [SerializeField] private float moveSpeed = 1.0f;


    [Header("References")]
    public ABATower abaTower;
    public Rigidbody rb;
    public PolyNavAgent agent;
    public PolyNav2D map;

    private void Awake()
    {
        //agent.enabled = false;
        abaUnitState = AbaUnitState.ROAMING;
    }

    public override void Start()
    {
        base.Start();
        agent.map = abaTower.roamingMap;
        var targetPoint = abaTower.GetPointWithinInfluence();
        agent.SetDestination(targetPoint);
    }
    
    public void Patrol()
    {
        // if (!hasTargetRoamingPoint)
        // {
        //     var targetPoint = abaTower.GetPointWithinInfluence();
        //     agent.SetDestination(targetPoint);
        //     hasTargetRoamingPoint = true;
        //     var seq = LeanTween.sequence();
        //     var duration = UnityEngine.Random.Range(1.0f, roamingDuration);
        //     seq.append(duration);
        //     seq.append(() => { hasTargetRoamingPoint = false; });
        // }
    }

    public bool IsRoamingState() { return abaUnitState == AbaUnitState.ROAMING; }
    public bool IsCarryingEnemyState() { return abaUnitState == AbaUnitState.CARRYING_ENEMY; }
    public void SetCarryingEnemyState() { abaUnitState = AbaUnitState.CARRYING_ENEMY; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (!IsRoamingState())
        //     return;

        // if (other.gameObject.layer != 10)
        //     return;

        // SetCarryingEnemyState();
        // carriedEnemy = other.transform.parent.GetComponent<BasicEnemy>();
        // carriedEnemy.StopMoving();
        // carriedEnemy.transform.SetParent(transform);
        // LeanTween.cancel(gameObject);
        // agent.SetDestination(GameManager.Instance.playerBase.transform.position);
        // hasTargetRoamingPoint = false;
    }
    
    private void OnDestinationReached()
    {
        if (IsRoamingState())
        {
            LeanTween.delayedCall(0.1f, () => {
                var targetPoint = abaTower.GetPointWithinInfluence();
                agent.SetDestination(targetPoint);
            });
        }
    }

    private void OnEnable()
    {
        agent.OnDestinationReached += OnDestinationReached;
    }

    private void OnDisable()
    {
        agent.OnDestinationReached -= OnDestinationReached;
    }
}
}
