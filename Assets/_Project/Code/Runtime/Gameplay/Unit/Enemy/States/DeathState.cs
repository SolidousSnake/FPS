using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.Speaker;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class DeathState : IState
    {
        private readonly EnemySpeaker _speaker;

        public DeathState(EnemySpeaker speaker)
        {
            _speaker = speaker;
        }

        public void Enter()
        {
            _speaker.SpeakDeath();
        }

        public void Exit()
        {
        }
    }
}