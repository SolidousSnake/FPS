using System.Linq;
using System.Threading;
using _Project.Code.Runtime.Config.AI.Stats;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.Unit.AI.Sensor;
using _Project.Code.Runtime.Unit.Player;
using _Project.Code.Runtime.Unit.Speaker;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Code.Runtime.AI.States
{
    public class AttackState : IState
    {
        private readonly AiStatsConfig _statsConfig;
        
        private readonly StateMachine _fsm;
        private readonly VisionSensor _visionSensor;
        private readonly EnemySpeaker _enemySpeaker;
        private readonly Rig _aimLayer;

        private readonly PlayerFacade _playerFacade;

        private CancellationTokenSource _cts;

        public AttackState(AiStatsConfig statsConfig
            , StateMachine fsm
            , VisionSensor visionSensor
            , EnemySpeaker enemySpeaker
            , Rig aimLayer
            , PlayerFacade playerFacade)
        {
            _statsConfig = statsConfig;
            _fsm = fsm;
            _visionSensor = visionSensor;
            _enemySpeaker = enemySpeaker;
            _aimLayer = aimLayer;
            _playerFacade = playerFacade;
        }

        public void Enter()
        {
            _visionSensor.TargetLost += _fsm.Enter<SearchState>;
            _aimLayer.weight = Constants.Animation.IK.Enable;
            _cts = new CancellationTokenSource();

            _enemySpeaker.SpeakTargetSighted();
            Attack().Forget();
        }

        public void Exit()
        {
            _visionSensor.TargetLost -= _fsm.Enter<SearchState>;
            _cts.Cancel();
        }
        
        private async UniTask Attack()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (!_visionSensor.VisibleTargets.Contains(_playerFacade.gameObject)) 
                    _fsm.Enter<ChaseTargetState>();

                var distanceToTarget = Vector3.Distance(_visionSensor.transform.position, _playerFacade.transform.position);

                if (distanceToTarget > _statsConfig.MaxAttackDistance) 
                    _fsm.Enter<ChaseTargetState>();

                await UniTask.WaitForSeconds(_statsConfig.UpdateStateDelay);
            }
        }
    }
}