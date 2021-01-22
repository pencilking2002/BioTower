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

    [Header("Roaming Settings")]
    public float roamingDuration = 2.0f;
    //[HideInInspector] public bool hasTargetRoamingPoint;


    [Header("Carrying enemy state")]
    [SerializeField] private BasicEnemy carriedEnemy;
    [SerializeField] private float moveSpeed = 1.0f;


    [Header("References")]
    public ABATower abaTower;
    public Rigidbody rb;
    public PolyNavAgent agent;

    private void Awake()
    {
        abaUnitState = AbaUnitState.ROAMING;
    }

    public override void Start()
    {
        base.Start();
        SetNewDestination();
    }
    
    public void Patrol()
    {
        // if (!hasTargetRoamingPoint)
        // {
        //     // var targetPoint = abaTower.GetPointWithinInfluence();
        //     // hasTargetRoamingPoint = true;
        //     // var seq = LeanTween.sequence();
        //     // var duration = UnityEngine.Random.Range(1.0f, roamingDuration);
        //     // seq.append(LeanTween.move(gameObject, targetPoint, duration));
        //     // seq.append(() => { hasTargetRoamingPoint = false; });
        // }
    }

    public bool IsRoamingState() { return abaUnitState == AbaUnitState.ROAMING; }
    public bool IsCarryingEnemyState() { return abaUnitState == AbaUnitState.CARRYING_ENEMY; }
    public void SetCarryingEnemyState() { abaUnitState = AbaUnitState.CARRYING_ENEMY; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsRoamingState())
            return;

        if (other.gameObject.layer != 10)
            return;
        
        SetCarryingEnemyState();
        agent.map = GameManager.Instance.levelMap.map;
        carriedEnemy = other.transform.parent.GetComponent<BasicEnemy>();
        carriedEnemy.StopMoving();
        carriedEnemy.transform.SetParent(transform);
        LeanTween.cancel(gameObject);
        agent.SetDestination(GameManager.Instance.playerBase.transform.position);
    }

    private void SetNewDestination()
    {
        var newDestination = abaTower.GetPointWithinInfluence();
        agent.SetDestination(newDestination);
    }

    private void OnDestinationReached()
    {
        if (IsRoamingState())
        {
            SetNewDestination();
        }
    }

    private void OnEnable()
    {
        agent.OnDestinationReached += OnDestinationReached;
        agent.OnDestinationInvalid += OnDestinationReached; // Used for whyen the destination is inside na obstacle
    }

    private void OnDisable()
    {
        agent.OnDestinationReached -= OnDestinationReached;
        agent.OnDestinationInvalid -= OnDestinationReached;
    }
}
}
