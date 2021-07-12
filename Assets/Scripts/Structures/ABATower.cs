﻿using System.Collections.Generic;
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
    //private Vector3[] targetPositions;
 

    public override void Awake()
    {
        base.Awake();
        Util.ScaleUpSprite(sr, 1.1f);
    }

    public override void Start()
    {
        base.Start();
        map.GenerateMap();      // NOTE: Seems like this needs to be called in order for the map to be initialized correctly after instantiation 
        var unitsContainer = transform.Find("Units");
        
        //targetPositions = new Vector3[numUnitsToSpawn];
        SpawnUnits(numUnitsToSpawn);

        Debug.Log("ABA tower Awake");
    }

    public override void OnUpdate()
    {
        influenceDisc.transform.eulerAngles += new Vector3(0,0,discRotateSpeed * Time.deltaTime);
    }

    public override void SpawnUnits(int numUnits)
    {
        for(int i=0; i<numUnits; i++)
        {
            var go = Instantiate(abaUnitPrefab);
            //go.transform.position = GetPointWithinInfluence();
            go.transform.position = GetPointWithinInfluence(); 
                
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

    // public override void OnTapStructure(Vector3 screenPos)
    // {
    //     // var oldScale = sr.transform.localScale;
    //     // LeanTween.scale(sr.gameObject, oldScale * 1.1f, 0.1f).setLoopPingPong(1);
    //     // SpawnUnits(1);
    //     //Debug.Log("tap structure");
    //     //towerMenu.OnTapStructure(structureType, screenPos);
    // }

    public Vector2 GetPointWithinInfluence()
    {
        return Util.GetPointWithinInfluence(
                minInfluenceAreaCollider.transform.position, 
                minInfluenceAreaCollider.radius, 
                maxInfluenceAreaCollider.radius
            );
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

