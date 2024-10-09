using System.Threading;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.Player;
using _Project.Code.Runtime.Unit.Speaker;
using _Project.Code.Runtime.Unit.Movement;
using _Project.Code.Runtime.Config.AI.Stats;
using _Project.Code.Runtime.Unit.Enemy.Install;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.AI.States
{
    public class ChaseTargetState : IState
    {
        private readonly AiStatsConfig _statsConfig;

        private readonly StateMachine _fsm;
        private readonly EnemySpeaker _speaker;
        private readonly NavMeshMovement _navMeshMovement;
        private readonly EnemyFacade _selfObject;

        private readonly PlayerFacade _playerFacade;
        
        private CancellationTokenSource _cts;

        public ChaseTargetState(AiStatsConfig statsConfig
            , StateMachine fsm
            , EnemySpeaker speaker
            , NavMeshMovement navMeshMovement
            , EnemyFacade selfObject
            , PlayerFacade playerFacade)
        {
            _statsConfig = statsConfig;
            _fsm = fsm;
            _speaker = speaker;
            _navMeshMovement = navMeshMovement;
            _selfObject = selfObject;
            _playerFacade = playerFacade;
        }

        public void Enter()
        {
            _cts = new CancellationTokenSource();

            _speaker.SpeakChase();
            Chase().Forget();
        }

        public void Exit()
        {
            _cts.Cancel();
        }

        private async UniTask Chase()
        {
            float distanceToTarget = 0f;

            while (!_cts.IsCancellationRequested)
            {
                _navMeshMovement.SetDestination(_playerFacade.transform.position);
                distanceToTarget = Vector3.Distance(_selfObject.transform.position, _playerFacade.transform.position);

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