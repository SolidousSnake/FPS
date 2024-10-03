using System.Threading;
using UnityEngine.Animations.Rigging;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.Movement;
using _Project.Code.Runtime.Unit.AI.Sensor;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Runtime.AI.States
{
    public class PatrolState : IDefaultState
    {
        private readonly StateMachine _stateMachine;
        private readonly VisionSensor _visionSensor;
        private readonly Rig _aimLayer;
        
        private readonly WaypointMovement _waypointMovement;
        private readonly NavMeshMovement _navMeshMovement;

        private CancellationTokenSource _cts;
        private int _waypointIndex;

        public PatrolState(StateMachine stateMachine
            , VisionSensor visionSensor
            , Rig aimLayer
            , WaypointMovement waypointMovement
            , NavMeshMovement navMeshMovement)
        {
            _stateMachine = stateMachine;
            _visionSensor = visionSensor;
            _aimLayer = aimLayer;
            _waypointMovement = waypointMovement;
            _navMeshMovement = navMeshMovement;
        }

        public void Enter()
        {
            _visionSensor.TargetSighted += _stateMachine.Enter<ChaseTargetState>;
            _aimLayer.weight = Constants.Animation.IK.Disable;
            _cts = new CancellationTokenSource();

            SetClosestWaypoint();
            Move();
            Patrol().Forget();
        }

        public void Exit()
        {
            _visionSensor.TargetSighted -= _stateMachine.Enter<ChaseTargetState>;
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
                    Move();
                }
                await UniTask.WaitForSeconds(_waypointMovement.GetWaitingTime());
            }
        }

        private void Move() => _navMeshMovement.SetDestination(_waypointMovement.GetCurrentWaypoint().position);

        private void SetClosestWaypoint() => 
            _waypointMovement.SetCurrentWaypoint(_waypointMovement.GetClosestWaypoint(_navMeshMovement.GetCurrentPosition()));
    }
}