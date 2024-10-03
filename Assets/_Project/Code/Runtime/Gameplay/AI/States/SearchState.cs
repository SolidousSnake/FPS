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
    public class SearchState : IState
    {
        private readonly EnemyCollection _enemyCollection;
        private readonly AiStatsConfig _statsConfig;
        
        private readonly StateMachine _fsm;
        private readonly VisionSensor _visionSensor;
        private readonly EnemySpeaker _speaker;

        private readonly NavMeshMovement _navMeshMovement;
        
        private readonly PlayerFacade _playerFacade;

        private Vector3 _lastKnownPlayerPosition;
        private CancellationTokenSource _cts;

        public SearchState(EnemyCollection enemyCollection
            ,AiStatsConfig statsConfig
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

        public void Enter()
        {
            _visionSensor.TargetSighted += _enemyCollection.ReportPlayerSighted;
            
            _lastKnownPlayerPosition = _playerFacade.transform.position;
            _cts = new CancellationTokenSource();
            
            _speaker.SpeakSearch();
            Search().Forget();
        }

        public void Exit()
        {
            _visionSensor.TargetSighted -= _enemyCollection.ReportPlayerSighted;
            _cts.Cancel();
        }

        private async UniTask Search()
        {
            while (!_cts.IsCancellationRequested)
            {
                _navMeshMovement.SetDestination(_lastKnownPlayerPosition);
                await UniTask.WaitForSeconds(_statsConfig.UpdateStateDelay);
            }
        }
    }
}