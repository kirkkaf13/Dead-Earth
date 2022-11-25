using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgent : MonoBehaviour
{

    public AIWaypointNetwork WaypointNetwork = null;
    public int CurrentWaypoint = 0;

    private NavMeshAgent _agent = null;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (WaypointNetwork == null) return;

        SetNextDestination(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (!_agent.hasPath && !_agent.pathPending || _agent.pathStatus != NavMeshPathStatus.PathComplete)
            SetNextDestination(true);
        else if (_agent.isPathStale)
            SetNextDestination(false);

    }

    void SetNextDestination(bool increment)
    {
        int incrementStep = increment ? 1 : 0;
        Transform nextWaypointTramsform = null;

        while (nextWaypointTramsform == null)
        {

            int nextDestinationIndex = (CurrentWaypoint + incrementStep >= WaypointNetwork.Waypoints.Count) ? 0 : CurrentWaypoint + incrementStep;
            nextWaypointTramsform = WaypointNetwork.Waypoints[nextDestinationIndex];

            if (nextWaypointTramsform != null)
            {
                CurrentWaypoint = nextDestinationIndex;
                _agent.destination = nextWaypointTramsform.position;
            }
        }
    }
}
