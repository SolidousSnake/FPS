using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using _Project.Code.Runtime.Unit.AI.Sensor;
using UnityEngine.Animations.Rigging;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class SentryState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly VisionSensor _visionSensor;
        private readonly Rig _aimLayer;

        public SentryState(StateMachine stateMachine, VisionSensor visionSensor, Rig aimLayer)
        {
            _stateMachine = stateMachine;
            _visionSensor = visionSensor;
            _aimLayer = aimLayer;
        }

        public void Enter()
        {
            _visionSensor.TargetSighted += EnterAttackState;
            _aimLayer.weight = Constants.Animation.IK.Disable;
        }

        public void Exit() => _visionSensor.TargetSighted -= EnterAttackState;

        private void EnterAttackState() => _stateMachine.Enter<AttackState>();
    }
}