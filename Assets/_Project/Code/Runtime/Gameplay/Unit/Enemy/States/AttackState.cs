using UnityEngine.Animations.Rigging;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.AI.Sensor;
using _Project.Code.Runtime.Unit.Speaker;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class AttackState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly VisionSensor _visionSensor;
        private readonly EnemySpeaker _enemySpeaker;
        private readonly Rig _aimLayer;

        public AttackState(StateMachine stateMachine, VisionSensor visionSensor, EnemySpeaker enemySpeaker, Rig aimLayer)
        {
            _stateMachine = stateMachine;
            _visionSensor = visionSensor;
            _enemySpeaker = enemySpeaker;
            _aimLayer = aimLayer;
        }

        public void Enter()
        {
            _aimLayer.weight = 1f;
            _enemySpeaker.SpeakTargetSighted();
            _visionSensor.TargetLost += EnterPreviousState;
        }

        public void Exit()
        {
            _aimLayer.weight = 0f;
            _visionSensor.TargetLost -= EnterPreviousState;
        }

        private void EnterPreviousState() => _stateMachine.EnterPreviousState();
    }
}