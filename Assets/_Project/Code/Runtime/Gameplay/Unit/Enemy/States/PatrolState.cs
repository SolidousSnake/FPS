using System.Threading;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.Unit.AI.Sensor;
using _Project.Code.Runtime.Unit.Movement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class PatrolState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly VisionSensor _visionSensor;
        private readonly WaypointMovement _waypointMovement;
        private readonly NavMeshMovement _navMeshMovement;
        private readonly Rig _aimLayer;

        private int _waypointIndex;
        private CancellationTokenSource _cts;

        public PatrolState(StateMachine stateMachine, VisionSensor visionSensor, WaypointMovement waypointMovement,
            NavMeshMovement navMeshMovement, Rig aimLayer)
        {
            _stateMachine = stateMachine;
            _visionSensor = visionSensor;
            _waypointMovement = waypointMovement;
            _navMeshMovement = navMeshMovement;
            _aimLayer = aimLayer;
        }

        public void Enter()
        {
            _visionSensor.TargetSighted += EnterAttackState;
            _cts = new CancellationTokenSource();

            SetClosestWaypoint();
            MoveToNextWaypoint();
            Patrol().Forget();
            _aimLayer.weight = Constants.Animation.IK.Disable;
        }

        public void Exit()
        {
            _visionSensor.TargetSighted -= EnterAttackState;
            _cts.Cancel();
            _navMeshMovement.Stop();
        }

        private async UniTask Patrol()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (_navMeshMovement.HasReachedDestination())
                {
                    _waypointMovement.MoveToNextWaypoint();
                    MoveToNextWaypoint();
                }
                await UniTask.WaitForSeconds(_waypointMovement.GetWaitingTime());
            }
        }

        private void MoveToNextWaypoint()
        {
            Transform nextWaypoint = _waypointMovement.GetCurrentWaypoint();
            _navMeshMovement.SetDestination(nextWaypoint.position);
        }

        private void SetClosestWaypoint()
        {
            Transform closestWaypoint = _waypointMovement.GetClosestWaypoint(_navMeshMovement.GetCurrentPosition());
            _waypointMovement.SetCurrentWaypoint(closestWaypoint);
        }

        private void EnterAttackState() => _stateMachine.Enter<AttackState>();
    }
}