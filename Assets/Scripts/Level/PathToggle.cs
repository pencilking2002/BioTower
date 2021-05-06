using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using NaughtyAttributes;

namespace BioTower
{
public class PathToggle : MonoBehaviour
{
    [ReorderableList][SerializeField] private GameObject[] dynObstacles;
    [MinMaxSlider(0, 5)][SerializeField] private Vector2 minMaxToggleInterval = new Vector2(5.0f, 10.0f);
    private float lastToggleTime;
    [SerializeField] private float currInterval;


    private void Awake()
    {
        int index = UnityEngine.Random.Range(0,2);
        dynObstacles[index].SetActive(false);    
    }

    private void Update()
    {
        if (Time.time > lastToggleTime + currInterval)
        {
            dynObstacles[0].SetActive(!dynObstacles[0].activeInHierarchy);
            dynObstacles[1].SetActive(!dynObstacles[1].activeInHierarchy);
            currInterval = UnityEngine.Random.Range(minMaxToggleInterval.x, minMaxToggleInterval.y);
            lastToggleTime = Time.time;
            EventManager.Game.onTogglePaths?.Invoke();
        }
    }

}
}