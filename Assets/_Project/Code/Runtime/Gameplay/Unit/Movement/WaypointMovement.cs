using System.Collections.Generic;
using _Project.Code.Runtime.Point.Waypoint;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Movement
{
    public class WaypointMovement
    {
        private readonly IReadOnlyList<Waypoint> _waypoints;
        private readonly MovementMode _movementMode;
        
        private int _currentWaypointIndex;
        private bool _isReversing;
        
        public WaypointMovement(IReadOnlyList<Waypoint> waypoints, MovementMode movementMode)
        {
            _waypoints = waypoints;
            _movementMode = movementMode;
            _currentWaypointIndex = 0;
            _isReversing = false;
        }

        public float GetWaitingTime() => _waypoints[_currentWaypointIndex].WaitingTime;
        public Transform GetCurrentWaypoint() => _waypoints[_currentWaypointIndex].transform;

        public void SetCurrentWaypoint(Transform waypoint)
        {
            for (int i = 0; i < _waypoints.Count; i++)
            {
                if (_waypoints[i].transform == waypoint)
                {
                    _currentWaypointIndex = i;
                    break;
                }
            }
        }

        public Transform GetClosestWaypoint(Vector3 position)
        {
            float closestDistance = float.MaxValue;
            Transform closestWaypoint = null;

            foreach (var waypoint in _waypoints)
            {
                float distance = Vector3.Distance(position, waypoint.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestWaypoint = waypoint.transform;
                }
            }

            return closestWaypoint;
        }

        public void MoveToNextWaypoint()
        {
            switch (_movementMode)
            {
                case MovementMode.Loop:
                    MoveInLoop();
                    break;
                case MovementMode.PingPong:
                    MoveInPingPong();
                    break;
            }
        }

        private void MoveInLoop() => _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;

        private void MoveInPingPong()
        {
            if (_isReversing)
            {
                _currentWaypointIndex--;
                if (_currentWaypointIndex <= 0)
                {
                    _currentWaypointIndex = 0;
                    _isReversing = false;
                }
            }
            else
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _waypoints.Count - 1)
                {
                    _currentWaypointIndex = _waypoints.Count - 1;
                    _isReversing = true;
                }
            }
        }
    }
}