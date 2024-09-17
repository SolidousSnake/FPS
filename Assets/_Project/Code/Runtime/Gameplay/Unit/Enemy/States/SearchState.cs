using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.Speaker;

namespace _Project.Code.Runtime.Unit.Enemy.States
{
    public class SearchState : IState
    {
        private readonly EnemySpeaker _speaker;

        public SearchState(EnemySpeaker speaker)
        {
            _speaker = speaker;
        }

        public void Enter()
        {
            _speaker.SpeakSearch();
        }

        public void Exit()
        {
        }
    }
}