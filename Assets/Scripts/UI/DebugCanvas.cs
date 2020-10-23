using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyNav;

namespace BioTower.UI
{
public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private PolyNav2D map;
    [SerializeField] private Transform endPoint;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        var enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.position = GameObject.FindGameObjectWithTag(Constants.enemyTestSpawnSpot).transform.position;
        // var agent = enemyGO.GetComponent<PolyNavAgent>();
        // agent.map = map;
        // agent.SetDestination(endPoint.position);
    }

    public void ReloadLevel()
    {
        GameManager.Instance.LoadLevel(0);
    }
}
}
