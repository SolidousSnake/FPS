using System.Threading;
using _Project.Code.Runtime.Config.AI.Stats;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.Movement;
using _Project.Code.Runtime.Unit.Player;
using _Project.Code.Runtime.Unit.Speaker;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.AI.States
{
    public class SearchState : IState
    {
        private readonly AiStatsConfig _statsConfig;
        
        private readonly EnemySpeaker _speaker;

        private readonly NavMeshMovement _navMeshMovement;
        
        private readonly PlayerFacade _playerFacade;

        private Vector3 _lastKnownPlayerPosition;
        private CancellationTokenSource _cts;

        public SearchState(AiStatsConfig statsConfig
            , EnemySpeaker speaker
            , NavMeshMovement navMeshMovement
            , PlayerFacade playerFacade)
        {
            _statsConfig = statsConfig;
            _speaker = speaker;
            _navMeshMovement = navMeshMovement;
            _playerFacade = playerFacade;
        }

        public void Enter()
        {
            _lastKnownPlayerPosition = _playerFacade.transform.position;
            _cts = new CancellationTokenSource();

            _speaker.SpeakSearch();
            Search().Forget();
        }

        public void Exit() => _cts?.Cancel();

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