﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyNav;
using BioTower.Structures;

namespace BioTower.UI
{
public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private PolyNav2D map;
    [SerializeField] private Transform endPoint;


    [Header("Text")]
    [SerializeField] private Text currWaveText;
    [SerializeField] private Text placementStateText;

    [SerializeField] private GameObject testMapPrefab;
  

    private void Awake()
    {
        placementStateText.text = "Placement state: NONE";
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnEnemy();
        }

        currWaveText.text = "Curr Wave: " + GameManager.Instance.waveManager.currWave;
    }

    public void SpawnTestMap()
    {
        Instantiate(testMapPrefab);
    }

    public void SpawnEnemy()
    {
        var enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.position = GameObject.FindGameObjectWithTag(Constants.enemyTestSpawnSpot).transform.position;
    }

    public void ReloadLevel()
    {
        GameManager.Instance.LoadLevel(0);
    }

    private void OnStartPlacementState(StructureType structureType)
    {
        placementStateText.text = "Placement state: PLACING";
    }

    private void OnSetNonePlacementState()
    {
        placementStateText.text = "Placement state: NONE";
    }

    private void OnEnable()
    {
        EventManager.Structures.onSetNonePlacementState += OnSetNonePlacementState;
        EventManager.Structures.onStartPlacementState += OnStartPlacementState;

    }

    private void OnDisable()
    {
        EventManager.Structures.onSetNonePlacementState -= OnSetNonePlacementState;
        EventManager.Structures.onStartPlacementState -= OnStartPlacementState;
    }
}
}
