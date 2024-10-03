using System.Threading;
using UnityEngine;
using _Project.Code.Runtime.Config.AI.Stats;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Collection;
using _Project.Code.Runtime.Unit.AI.Sensor;
using _Project.Code.Runtime.Unit.Movement;
using _Project.Code.Runtime.Unit.Player;
using _Project.Code.Runtime.Unit.Speaker;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.AI.States
{
    public class ChaseTargetState : IState
    {
        private readonly EnemyCollection _enemyCollection;
        private readonly AiStatsConfig _statsConfig;

        private readonly StateMachine _fsm;
        private readonly VisionSensor _visionSensor;
        private readonly EnemySpeaker _speaker;

        private readonly NavMeshMovement _navMeshMovement;

        private readonly PlayerFacade _playerFacade;

        public ChaseTargetState( EnemyCollection enemyCollection
            , AiStatsConfig statsConfig
            , StateMachine fsm
            , VisionSensor visionSensor
            , EnemySpeaker speaker
            , NavMeshMovement navMeshMovement
            , PlayerFacade playerFacade)
        {
            _enemyCollection = enemyCollection;
            _statsConfig = statsConfig;
            _fsm = fsm;
            _visionSensor = visionSensor;
            _speaker = speaker;
            _navMeshMovement = navMeshMovement;
            _playerFacade = playerFacade;
        }

        private CancellationTokenSource _cts;

        public void Enter()
        {
            _cts = new CancellationTokenSource();
            // _visionSensor.TargetLost += _fsm.Enter<SearchState>;
            _visionSensor.TargetLost += _enemyCollection.ReportPlayerLost;

            _speaker.SpeakChase();
            Chase().Forget();
        }

        public void Exit()
        {
            // _visionSensor.TargetLost -= _fsm.Enter<SearchState>;
            _visionSensor.TargetLost -= _enemyCollection.ReportPlayerLost;
            _cts.Cancel();
        }

        private async UniTask Chase()
        {
            float distanceToTarget = 0f;

            while (!_cts.IsCancellationRequested)
            {
                _navMeshMovement.SetDestination(_playerFacade.transform.position);
                distanceToTarget = Vector3.Distance(_visionSensor.transform.position, _playerFacade.transform.position);

                if (distanceToTarget <= _statsConfig.MaxAttackDistance)
                {
                    _navMeshMovement.Stop();
                    _fsm.Enter<AttackState>();
                }

                await UniTask.WaitForSeconds(_statsConfig.UpdateStateDelay);
            }
        }
    }
}