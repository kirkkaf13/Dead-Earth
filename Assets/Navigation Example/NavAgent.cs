using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgent : MonoBehaviour
{

    public AIWaypointNetwork WaypointNetwork = null;
    public int CurrentWaypoint = 0;
    public AnimationCurve animationCurve = null;

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

        if (_agent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(1.0f));
            return;
        }

        if (!_agent.hasPath && !_agent.pathPending || _agent.pathStatus != NavMeshPathStatus.PathComplete)
            SetNextDestination(true);
        else if (_agent.isPathStale)
            SetNextDestination(false);

    }

    IEnumerator Jump(float duration)
    {
        var data = _agent.currentOffMeshLinkData;
        var startPos = _agent.transform.position;
        var endPos = data.endPos + (_agent.baseOffset * Vector3.up);
        var time = 0.0f;

        while (time <= duration)
        {
            var t = time / duration;
            _agent.transform.position = Vector3.Lerp(startPos, endPos, t) + animationCurve.Evaluate(t) * Vector3.up;
            time += Time.deltaTime;
            yield return null;
        }
        _agent.CompleteOffMeshLink();
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
