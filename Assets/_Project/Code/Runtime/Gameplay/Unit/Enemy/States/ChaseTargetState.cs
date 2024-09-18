using System.Threading;
using _Project.Code.Runtime.Config.AI.Stats;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.AI.Action;
using _Project.Code.Runtime.Unit.AI.Sensor;
using _Project.Code.Runtime.Unit.Movement;
using _Project.Code.Runtime.Unit.Speaker;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class ChaseTargetState : IState
    {
        private readonly StateMachine _fsm;
        private readonly EnemySpeaker _speaker;
        private readonly VisionSensor _visionSensor;
        private readonly FindClosestTargetAction _findClosestTargetAction;
        private readonly AiStatsConfig _statsConfig;
        private readonly NavMeshMovement _navMeshMovement;
        
        private CancellationTokenSource _cts;

        public ChaseTargetState(StateMachine fsm, EnemySpeaker speaker, VisionSensor visionSensor
            , FindClosestTargetAction findClosestTargetAction, AiStatsConfig statsConfig, NavMeshMovement navMeshMovement)
        {
            _fsm = fsm;
            _speaker = speaker;
            _visionSensor = visionSensor;
            _findClosestTargetAction = findClosestTargetAction;
            _statsConfig = statsConfig;
            _navMeshMovement = navMeshMovement;
        }

        public void Enter()
        {
            _cts = new CancellationTokenSource();
            _visionSensor.TargetLost += EnterSearchState;
            
            _speaker.SpeakChase();
            Chase().Forget();
        }

        public void Exit()
        {
            _visionSensor.TargetLost -= EnterSearchState;
            _cts.Cancel();
        }

        private void EnterSearchState() => _fsm.Enter<SearchState>();
        
        private async UniTask Chase()
        {
            while (!_cts.IsCancellationRequested)
            {
                Transform currentTarget = _findClosestTargetAction.GetClosestTarget();
                float distanceToTarget = 0f;

                if (currentTarget != null)
                {
                    _navMeshMovement.SetDestination(currentTarget.position);
                    distanceToTarget = Vector3.Distance(_visionSensor.transform.position,currentTarget.position);

                    if (distanceToTarget <= _statsConfig.MaxAttackDistance)
                    {
                        _navMeshMovement.Stop();
                        _fsm.Enter<AttackState>();
                    }
                    await UniTask.WaitForSeconds(_statsConfig.UpdateStateDelay);
                }
                else
                {
                    _fsm.Enter<SearchState>();
                }
            }
        }
    }
}