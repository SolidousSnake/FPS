using _Project.Code.Runtime.Config.Player;
using _Project.Code.Runtime.Core.States;
using _Project.Code.Runtime.Services.Input;
using _Project.Code.Runtime.States;
using _Project.Code.Runtime.UI.View;
using _Project.Code.Runtime.UI.View.Crosshair;
using _Project.Code.Runtime.Unit.Rotator;
using _Project.Code.Runtime.Unit.Movement;
using _Project.Code.Runtime.Unit.Player.States;
using _Project.Code.Runtime.Weapon.Holder;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Unit.Player
{
    public sealed class PlayerFacade : MonoBehaviour
    {
        [Title("Components")] [SerializeField]
        private Rigidbody _rigidBody;

        [SerializeField] private PlayerSettings _settings;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _body;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private WeaponHolder _weaponHolder;

        [Title("States")] [SerializeField] private PlayerStateConfig _standState;
        [SerializeField] private PlayerStateConfig _crouchState;
        [SerializeField] private PlayerStateConfig _proneState;

        private InputService _inputService;
        private PhysicsMovement _movement;
        private CameraRotator _cameraRotator;
        private StateMachine _stateMachine;
        private PlayerWeaponHolderProvider _weaponHolderProvider;

        [Inject]
        private void Construct(InputService inputService, WeaponAmmoView weaponAmmoView, CrosshairView crosshairView)
        {
            _inputService = inputService;

            _movement = new PhysicsMovement(_rigidBody, _body);
            _cameraRotator = new CameraRotator(_head, _settings.MouseSettings, _body);
            _weaponHolderProvider = new PlayerWeaponHolderProvider(inputService, weaponAmmoView, crosshairView, _weaponHolder);

            RegisterStates();
            Subscribe();
        }

        private void RegisterStates()
        {
            _stateMachine = new StateMachine();

            _stateMachine.RegisterState(new StandingState(_stateMachine, _inputService, _standState, _collider,
                _movement));
            _stateMachine.RegisterState(new CrouchState(_stateMachine, _inputService, _crouchState, _collider,
                _movement));
            _stateMachine.RegisterState(new ProneState(_stateMachine, _inputService, _proneState, _collider,
                _movement));

            _stateMachine.Enter<StandingState>();
        }

        private void Subscribe()
        {
            _inputService.MovementButtonPressed += _movement.Move;
            _inputService.MouseDeltaChanged += _cameraRotator.SetRotation;
        }

        private void OnDestroy()
        {
            _inputService.MovementButtonPressed -= _movement.Move;
            _inputService.MouseDeltaChanged -= _cameraRotator.SetRotation;
        }
    }
}