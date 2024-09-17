using _Project.Code.Runtime.Config.Player;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Input;
using _Project.Code.Runtime.Unit.Movement;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Player.States
{
    public sealed class StandingState : PlayerBaseState
    {
        private readonly StateMachine _stateMachine;
        private readonly InputService _inputService;

        public StandingState(StateMachine stateMachine, InputService inputService, PlayerStateConfig stateConfig
            , CapsuleCollider collider, PhysicsMovement movement) : base(stateConfig, collider, movement)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
        }

        protected override void Subscribe()
        {
            _inputService.CrouchButtonPressed += EnterCrouchState;
            _inputService.ProneButtonPressed += EnterProneState;
        }

        protected override void Unsubscribe()
        {
            _inputService.CrouchButtonPressed -= EnterCrouchState;
            _inputService.ProneButtonPressed -= EnterProneState;
        }

        private void EnterCrouchState() => _stateMachine.Enter<CrouchState>();
        private void EnterProneState() => _stateMachine.Enter<ProneState>();
    }
}