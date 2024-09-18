using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.AI.Sensor;
using _Project.Code.Runtime.Unit.Speaker;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class SearchState : IState
    {
        private readonly StateMachine _fsm;
        private readonly VisionSensor _visionSensor;
        private readonly EnemySpeaker _speaker;

        public SearchState(StateMachine fsm, VisionSensor visionSensor, EnemySpeaker speaker)
        {
            _fsm = fsm;
            _visionSensor = visionSensor;
            _speaker = speaker;
        }

        public void Enter()
        {
            _visionSensor.TargetSighted += EnterAttackState;
            _speaker.SpeakSearch();
        }

        public void Exit() => _visionSensor.TargetSighted -= EnterAttackState;

        private void EnterAttackState() => _fsm.Enter<AttackState>();
    }
}