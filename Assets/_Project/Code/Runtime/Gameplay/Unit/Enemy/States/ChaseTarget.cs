using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.Speaker;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class ChaseTarget : IState
    {
        private readonly EnemySpeaker _speaker;

        public ChaseTarget(EnemySpeaker speaker)
        {
            _speaker = speaker;
        }

        public void Enter()
        {
            _speaker.SpeakChase();
        }

        public void Exit()
        {
        }
    }
}