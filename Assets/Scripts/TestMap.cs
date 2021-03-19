using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioTower.Units;
using PolyNav;


namespace BioTower
{
public class TestMap : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    public PolyNavAgent agent;
    private PolyNav2D map;
    

    private void Awake()
    {
        map = GetComponent<PolyNav2D>();
        //map.GenerateMap();
        agent.map = map;
        var destination = (Vector2) agent.transform.position + offset;
        agent.SetDestination(destination);
    }
}
}