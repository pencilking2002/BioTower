using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using PolyNav;
using Shapes;

namespace BioTower.Structures
{

[SelectionBase]
public class ABATower : Structure
{   
    [SerializeField] private float discRotateSpeed = 2;
    [SerializeField] private int numUnitsToSpawn = 4;
    [SerializeField] private List<AbaUnit> abaUnits;


    [Header("References")]
    [SerializeField] private Disc influenceDisc;
    [SerializeField] private GameObject abaUnitPrefab;
    public PolyNav2D map;
    [SerializeField] private Transform unitsContainer;
    [SerializeField] private CircleCollider2D maxInfluenceAreaCollider;
    [SerializeField] private CircleCollider2D minInfluenceAreaCollider;
 

    private void Awake()
    {
        Util.ScaleUpSprite(sr, 1.1f);
    }

    private void Start()
    {
        map.GenerateMap();      // NOTE: Seems like this needs to be called in order for the map to be initialized correctly after instantiation 
        var unitsContainer = transform.Find("Units");
        
        //targetPositions = new Vector3[numUnitsToSpawn];
        SpawnUnits(numUnitsToSpawn);

        Debug.Log("ABA tower Awake");
    }

    public override void OnUpdate()
    {
        if (GameManager.Instance.gameStates.gameState != GameState.GAME)
            return;
            
        influenceDisc.transform.eulerAngles += new Vector3(0,0,discRotateSpeed * Time.deltaTime);
    }

    public override void SpawnUnits(int numUnits)
    {
        for(int i=0; i<numUnits; i++)
        {
            var go = Instantiate(abaUnitPrefab);
            //go.transform.position = GetPointWithinInfluence();
            go.transform.position = GetEdgePointWithinInfluence(); 
                
            go.transform.SetParent(unitsContainer);
            var unit = go.GetComponent<AbaUnit>();
            unit.agent.map = map; 
            unit.abaTower = this;
            AddUnit(unit);
        }
    }

    public void AddUnit(AbaUnit unit)
    {
        abaUnits.Add(unit);
    }

    public void RemoveUnit(AbaUnit unit)
    {
        abaUnits.Remove(unit);
    }

    public Vector2 GetEdgePointWithinInfluence()
    {
        if (minInfluenceAreaCollider == null || maxInfluenceAreaCollider == null)
            return Vector2.zero;
            
        var point = Util.GetPointWithinInfluence(
                minInfluenceAreaCollider.transform.position, 
                minInfluenceAreaCollider.radius, 
                maxInfluenceAreaCollider.radius
            );
        Vector2 center = minInfluenceAreaCollider.transform.position;
        Vector2 direction = (point-center).normalized;
        point = center + direction * maxInfluenceAreaCollider.radius;
        return point;
    }
    
    private void OnDrawGizmosSelected()
    {
        var minRadius = minInfluenceAreaCollider.radius * minInfluenceAreaCollider.transform.lossyScale.x;
        var maxRadius = maxInfluenceAreaCollider.radius * maxInfluenceAreaCollider.transform.lossyScale.x;
        
        var color = Color.red;
        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawSphere(minInfluenceAreaCollider.transform.position, minRadius);

        color = Color.blue;
        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawSphere(maxInfluenceAreaCollider.transform.position, maxRadius);
    }
}
}

