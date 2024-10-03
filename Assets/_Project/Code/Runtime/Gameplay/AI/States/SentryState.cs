using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.Services.Collection;
using _Project.Code.Runtime.Unit.AI.Sensor;
using UnityEngine.Animations.Rigging;

namespace _Project.Code.Runtime.AI.States
{
    public class SentryState : IDefaultState
    {
        private readonly EnemyCollection _enemyCollection;
        
        private readonly StateMachine _stateMachine;
        private readonly VisionSensor _visionSensor;
        private readonly Rig _aimLayer;

        public SentryState(EnemyCollection enemyCollection, StateMachine stateMachine, VisionSensor visionSensor, Rig aimLayer)
        {
            _enemyCollection = enemyCollection;
            _stateMachine = stateMachine;
            _visionSensor = visionSensor;
            _aimLayer = aimLayer;
        }

        public void Enter()
        {
            // _visionSensor.TargetSighted += _stateMachine.Enter<ChaseTargetState>;
            _visionSensor.TargetSighted += _enemyCollection.ReportPlayerSighted;
            _aimLayer.weight = Constants.Animation.IK.Disable;
        }

        // public void Exit() => _visionSensor.TargetSighted -= _stateMachine.Enter<ChaseTargetState>;
        public void Exit() => _visionSensor.TargetSighted -= _enemyCollection.ReportPlayerSighted;
    }
}