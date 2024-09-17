using UnityEngine;
using UnityEngine.AI;

namespace _Project.Code.Runtime.Unit.Movement
{
    public class NavMeshMovement
    {
        private readonly NavMeshAgent _navMeshAgent;

        public NavMeshMovement(NavMeshAgent navMeshAgent)
        {
            _navMeshAgent = navMeshAgent;
        }

        public void SetDestination(Vector3 position) => _navMeshAgent.SetDestination(position);
        public void Stop() => _navMeshAgent.ResetPath();

        public bool HasReachedDestination() =>
            !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
            (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f);

        public Vector3 GetCurrentPosition() => _navMeshAgent.transform.position;
    }
}