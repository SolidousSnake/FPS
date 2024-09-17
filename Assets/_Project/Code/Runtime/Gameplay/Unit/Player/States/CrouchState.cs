using _Project.Code.Runtime.Config.Player;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Input;
using _Project.Code.Runtime.Unit.Movement;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Player.States
{
    public sealed class CrouchState : PlayerBaseState
    {
        private readonly StateMachine _stateMachine;
        private readonly InputService _inputService;
        
        public CrouchState(StateMachine stateMachine, InputService inputService, PlayerStateConfig stateConfig
            , CapsuleCollider collider, PhysicsMovement movement) : base(stateConfig, collider, movement)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
        }

        protected override void Subscribe()
        {
            _inputService.CrouchButtonPressed += EnterStandingState;
            _inputService.ProneButtonPressed += EnterProneState;
        }

        protected override void Unsubscribe()
        {
            _inputService.CrouchButtonPressed -= EnterStandingState;
            _inputService.ProneButtonPressed -= EnterProneState;
        }
        
        private void EnterStandingState() => _stateMachine.Enter<StandingState>();
        private void EnterProneState() => _stateMachine.Enter<ProneState>();
    }
}