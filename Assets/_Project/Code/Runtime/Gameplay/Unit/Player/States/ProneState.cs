using _Project.Code.Runtime.Config.Player;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Input;
using _Project.Code.Runtime.Unit.Movement;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Player.States
{
    public sealed class ProneState : PlayerBaseState
    {
        private readonly StateMachine _stateMachine;
        private readonly InputService _inputService;

        public ProneState(StateMachine stateMachine, InputService inputService, PlayerStateConfig stateConfig
            , CapsuleCollider collider, PhysicsMovement movement) : base(stateConfig, collider, movement)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
        }

        protected override void Subscribe()
        {
            _inputService.CrouchButtonPressed += EnterCrouchState;
            _inputService.ProneButtonPressed += EnterStandingState;
        }

        protected override void Unsubscribe()
        {
            _inputService.CrouchButtonPressed -= EnterCrouchState;
            _inputService.ProneButtonPressed -= EnterStandingState;
        }

        private void EnterCrouchState() => _stateMachine.Enter<CrouchState>();
        private void EnterStandingState() => _stateMachine.Enter<StandingState>();
    }
}