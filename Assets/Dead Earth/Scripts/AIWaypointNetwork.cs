using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathDisplayMode { None, Connections, Navigation }
public class AIWaypointNetwork : MonoBehaviour
{
    public PathDisplayMode PathDisplayMode = PathDisplayMode.None;
    public List<Transform> Waypoints = new List<Transform>();
   
}
