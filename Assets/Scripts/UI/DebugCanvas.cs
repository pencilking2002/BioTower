using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BioTower.UI
{
public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    public void SpawnEnemy()
    {
        var enemyGO = Instantiate(enemyPrefab);
        enemyGO.transform.position = GameObject.FindGameObjectWithTag(Constants.enemyTestSpawnSpot).transform.position;
    }

    public void ReloadLevel()
    {
        GameManager.Instance.LoadLevel(0);
    }
}
}
