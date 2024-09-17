using _Project.Code.Runtime.Config.Player;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Unit.Movement;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Player.States
{
    public abstract class PlayerBaseState : IState
    {
        private readonly PlayerStateConfig _stateConfig;
        private readonly PhysicsMovement _movement;
        private readonly CapsuleCollider _collider;

        protected PlayerBaseState(PlayerStateConfig stateConfig, CapsuleCollider collider, PhysicsMovement movement)
        {
            _stateConfig = stateConfig;
            _movement = movement;
            _collider = collider;
        }
        
        public void Enter()
        {
            _movement.SetSpeed(_stateConfig.MovementSpeed);
            _collider.height = _stateConfig.ColliderHeight;
            _collider.center = _stateConfig.ColliderCenter;
          
            Subscribe();
        }

        public void Exit() => Unsubscribe();
        protected abstract void Subscribe();
        protected abstract void Unsubscribe();
    }
}