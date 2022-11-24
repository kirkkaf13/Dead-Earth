using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetworkEditor : Editor
{
    void OnSceneGUI()
    {

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.magenta;

        AIWaypointNetwork network = target as AIWaypointNetwork;
        network.Waypoints.ForEach(delegate (Transform t)
        {
            if (t != null)
                Handles.Label(t.position, t.name, style);
        });

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
    }
}
