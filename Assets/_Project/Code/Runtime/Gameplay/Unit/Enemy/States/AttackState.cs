using System.Linq;
using System.Threading;
using _Project.Code.Runtime.Config.AI.Stats;
using UnityEngine.Animations.Rigging;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.Unit.AI.Action;
using _Project.Code.Runtime.Unit.AI.Sensor;
using _Project.Code.Runtime.Unit.Speaker;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class AttackState : IState
    {
        private readonly StateMachine _fsm;
        private readonly VisionSensor _visionSensor;
        private readonly EnemySpeaker _enemySpeaker;
        private readonly AiStatsConfig _statsConfig;
        private readonly FindClosestTargetAction _findClosestTargetAction;
        private readonly Rig _aimLayer;

        private CancellationTokenSource _cts;

        public AttackState(StateMachine fsm, VisionSensor visionSensor, EnemySpeaker enemySpeaker,
            FindClosestTargetAction findClosestTargetAction, AiStatsConfig statsConfig, Rig aimLayer)
        {
            _fsm = fsm;
            _visionSensor = visionSensor;
            _enemySpeaker = enemySpeaker;
            _findClosestTargetAction = findClosestTargetAction;
            _statsConfig = statsConfig;
            _aimLayer = aimLayer;
        }

        public void Enter()
        {
            _visionSensor.TargetLost += EnterSearchState;
            _aimLayer.weight = Constants.Animation.IK.Enable;
            _cts = new CancellationTokenSource();

            _enemySpeaker.SpeakTargetSighted();
            Attack().Forget();
        }

        public void Exit()
        {
            _visionSensor.TargetLost -= EnterSearchState;
            _cts.Cancel();
        }
        
        private void EnterSearchState() => _fsm.Enter<SearchState>();
        
        private async UniTask Attack()
        {
            while (!_cts.IsCancellationRequested)
            {
                Transform target = _findClosestTargetAction.GetClosestTarget();

                if (target == null || !_visionSensor.VisibleTargets.Contains(target.gameObject)) 
                    _fsm.Enter<ChaseTargetState>();

                float distanceToTarget = Vector3.Distance(_visionSensor.transform.position, target.position);

                if (distanceToTarget > _statsConfig.MaxAttackDistance) 
                    _fsm.Enter<ChaseTargetState>();

                await UniTask.WaitForSeconds(_statsConfig.UpdateStateDelay);
            }
        }
    }
}