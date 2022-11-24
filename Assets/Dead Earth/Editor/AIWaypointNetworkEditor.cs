using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetworkEditor : Editor
{

    public override void OnInspectorGUI()
    {
        AIWaypointNetwork network = target as AIWaypointNetwork;
        network.PathDisplayMode = (PathDisplayMode)EditorGUILayout.EnumPopup("Display Mode", network.PathDisplayMode);
        DrawDefaultInspector();
        SceneView.RepaintAll();
    }

    void OnSceneGUI()
    {

        // Display waypoint names
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.magenta;

        AIWaypointNetwork network = target as AIWaypointNetwork;
        network.Waypoints.ForEach(delegate (Transform t)
        {
            if (t != null)
                Handles.Label(t.position, t.name, style);
        });

        // Display way point connections
        if (network.PathDisplayMode == PathDisplayMode.Connections)
        {

            Vector3[] points = new Vector3[network.Waypoints.Count + 1];
            for (int i = 0; i <= network.Waypoints.Count; i++)
            {
                int index = i != network.Waypoints.Count ? i : 0;
                if (network.Waypoints[index] != null)
                {
                    points[i] = network.Waypoints[index].position;
                }
                else
                {
                    points[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
                }

            }
            Handles.color = Color.cyan;
            Handles.DrawPolyLine(points);
        }
        // Display waypoint navigation
        else if (network.PathDisplayMode == PathDisplayMode.Navigation)
        {
            Handles.color = Color.cyan;
            for (int i = 1; i < network.Waypoints.Count + 1; i++)
            {
                int index = i != network.Waypoints.Count ? i : 0;
                if (network.Waypoints[index] != null)
                {
                    NavMeshPath path = new NavMeshPath();
                    NavMesh.CalculatePath(network.Waypoints[index].position, network.Waypoints[i - 1].position, NavMesh.AllAreas, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                        Handles.DrawAAPolyLine(path.corners);
                }
            }
        }
    }
}
