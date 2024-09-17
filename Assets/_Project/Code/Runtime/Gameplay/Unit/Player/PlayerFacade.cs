using _Project.Code.Runtime.Config.Player;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Input;
using _Project.Code.Runtime.Unit.Handlers;
using _Project.Code.Runtime.Unit.Movement;
using _Project.Code.Runtime.Unit.Player.States;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Unit.Player
{
    public sealed class PlayerFacade : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private PlayerSettings _settings;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _body;
        [SerializeField] private CapsuleCollider _collider;

        [Header("States")]
        [SerializeField] private PlayerStateConfig _standState;
        [SerializeField] private PlayerStateConfig _crouchState;
        [SerializeField] private PlayerStateConfig _proneState;

        private InputService _inputService;
        private PhysicsMovement _movement;
        private CameraHandler _cameraHandler;
        private StateMachine _stateMachine;
        
        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
        
            _movement = new PhysicsMovement(_rigidBody, _body);
            _cameraHandler = new CameraHandler(_head, _settings.MouseSettings, _body);
        
            RegisterStates();
            Subscribe();
        }

        private void RegisterStates()
        {
            _stateMachine = new StateMachine();
            
            _stateMachine.RegisterState(new StandingState(_stateMachine, _inputService, _standState, _collider, _movement));
            _stateMachine.RegisterState(new CrouchState(_stateMachine, _inputService, _crouchState, _collider, _movement));
            _stateMachine.RegisterState(new ProneState(_stateMachine, _inputService, _proneState, _collider, _movement));
            
            _stateMachine.Enter<StandingState>();
        }

        private void Subscribe()
        {
            _inputService.MovementButtonPressed += _movement.Move;
            _inputService.MouseDeltaChanged += _cameraHandler.SetRotation;
        }

        private void OnDestroy()
        {
            _inputService.MovementButtonPressed -= _movement.Move;
            _inputService.MouseDeltaChanged -= _cameraHandler.SetRotation;
        }
    }
}