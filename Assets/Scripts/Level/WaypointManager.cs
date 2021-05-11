using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using System.Collections.Generic;

namespace BioTower.Level
{
[ExecuteAlways]
public class WaypointManager : MonoBehaviour
{
    private Waypoint[] waypoints;
    private Waypoint[] spawnPoints;

    private void Awake()
    {
        CacheWaypoints();
        CacheSpawnPoints();
    }

    private void CacheWaypoints()
    {
        waypoints = new Waypoint[transform.childCount];
        for(int i=0; i<transform.childCount; i++)
        {
            waypoints[i] = transform.GetChild(i).GetComponent<Waypoint>();
        }  
    }

    private void CacheSpawnPoints()
    {
        List<Waypoint> spawnPointList = new List<Waypoint>();
        foreach(Waypoint waypoint in waypoints)
        {
            if (waypoint.isSpawnPoint)
                spawnPointList.Add(waypoint);
        }
        spawnPoints = spawnPointList.ToArray();
    }

    public Waypoint GetRandomSpawnPoint()
    {
        int randIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        return spawnPoints[randIndex];
    }

    #if UNITY_EDITOR

    [MenuItem("Tools/Link Waypoints %l")]
    private static void LinkWaypoints()
    {
        
        var selectedObjects = Selection.gameObjects.ToList<GameObject>();

        if (selectedObjects.Count == 2)
        {
            if (selectedObjects[0].layer != 15 || selectedObjects[1].layer != 15)
                return;
            
            var waypoint_02 = Selection.activeTransform.GetComponent<Waypoint>();
            selectedObjects.Remove(waypoint_02.gameObject);
            var waypoint_01 = selectedObjects[0].GetComponent<Waypoint>();
           
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Link Waypoints");
            var undoGroupIndex = Undo.GetCurrentGroup();
            Undo.RegisterCompleteObjectUndo(waypoint_01, "");
            Undo.RegisterCompleteObjectUndo(waypoint_02, "");
            Undo.CollapseUndoOperations(undoGroupIndex);

            waypoint_01.nextWaypoint = waypoint_02;
            waypoint_02.prevWaypoint = waypoint_01;
        }
        else if (selectedObjects.Count == 3)
        {
            
            var waypoint_01 = selectedObjects[2].GetComponent<Waypoint>();
            var waypoint_02 = selectedObjects[1].GetComponent<Waypoint>();
            var waypoint_03 = selectedObjects[0].GetComponent<Waypoint>();

            Undo.SetCurrentGroupName("Link Waypoints");
            var undoGroupIndex = Undo.GetCurrentGroup();
            Undo.RegisterCompleteObjectUndo(waypoint_01, "");
            Undo.RegisterCompleteObjectUndo(waypoint_02, "");
            Undo.RegisterCompleteObjectUndo(waypoint_03, "");
            Undo.CollapseUndoOperations(undoGroupIndex);

            waypoint_03.waypointType = WaypointType.FORK;
            waypoint_03.nextWaypoint = waypoint_02;
            waypoint_03.nextWaypoint_02 = waypoint_01;

            waypoint_01.prevWaypoint = waypoint_03;
            waypoint_02.prevWaypoint = waypoint_03;
        }
    }

    #endif

}
}