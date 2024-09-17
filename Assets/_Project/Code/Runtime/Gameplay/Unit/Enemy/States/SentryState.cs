using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.AI.Sensor;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class SentryState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly VisionSensor _visionSensor;

        public SentryState(StateMachine stateMachine, VisionSensor visionSensor)
        {
            _stateMachine = stateMachine;
            _visionSensor = visionSensor;
        }

        public void Enter()
        {
            _visionSensor.TargetSighted += EnterAttackState;
        }

        public void Exit()
        {
            _visionSensor.TargetSighted -= EnterAttackState;
        }

        private void EnterAttackState() => _stateMachine.Enter<AttackState>();
    }
}