using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Core.Utils;
using UnityEngine.Animations.Rigging;

namespace _Project.Code.Runtime.AI.States
{
    public class SentryState : IDefaultState
    {
        private readonly Rig _aimLayer;

        public SentryState(Rig aimLayer)
        {
            _aimLayer = aimLayer;
        }

        public void Enter()
        {
            _aimLayer.weight = Constants.Animation.IK.Disable;
        }

        public void Exit()
        {
        }
    }
}