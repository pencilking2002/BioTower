using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BioTower.Level
{
[ExecuteAlways]
public class Waypoints : MonoBehaviour
{

    [MenuItem("Tools/Link Waypoints %l")]
    private static void LinkWaypoints()
    {
        
        var selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 2)
        {
            if (selectedObjects[0].layer != 15 || selectedObjects[1].layer != 15)
                return;
            
            var waypoint_01 = selectedObjects[1].GetComponent<Waypoint>();
            var waypoint_02 = selectedObjects[0].GetComponent<Waypoint>();
            
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Link Waypoints");
            var undoGroupIndex = Undo.GetCurrentGroup();
            Undo.RegisterCompleteObjectUndo(waypoint_01, "");
            Undo.RegisterCompleteObjectUndo(waypoint_02, "");
            Undo.CollapseUndoOperations(undoGroupIndex);

            waypoint_01.nextWaypoint = waypoint_02;
            waypoint_02.prevWaypoint = waypoint_01;
        }
        else if (selectedObjects.Length == 3)
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

            waypoint_03.isFork = true;
            waypoint_03.nextWaypoint = waypoint_02;
            waypoint_03.nextWaypoint_02 = waypoint_01;

            waypoint_01.prevWaypoint = waypoint_03;
            waypoint_02.prevWaypoint = waypoint_03;
        }
    }
}
}