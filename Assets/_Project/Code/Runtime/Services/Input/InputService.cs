using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Code.Runtime.Services.Input
{
    public sealed class InputService : MonoBehaviour, ITickable
    {
        private Vector2 _movementVector;

        private bool _toggleAimMode = false;

        public event Action CrouchButtonPressed;
        public event Action ProneButtonPressed;
        public event Action Fire1Pressed;
        public event Action Fire2Pressed;
        public event Action ReloadButtonPressed;

        public event Action Fire1Released;
        public event Action Fire2Released;

        public event Action<int> WeaponSelectPressed;

        public event Action<Vector2> MouseDeltaChanged;
        public event Action<Vector3> MovementButtonPressed;

        public void OnMove(InputAction.CallbackContext ctx) => _movementVector = ctx.ReadValue<Vector2>();
        public void OnLook(InputAction.CallbackContext ctx) => MouseDeltaChanged?.Invoke(ctx.ReadValue<Vector2>());

        public void OnProne(InputAction.CallbackContext ctx) => ProneButtonPressed?.Invoke();

        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
                CrouchButtonPressed?.Invoke();
        }
        
        public void OnAim(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                Fire2Pressed?.Invoke();
            else if (ctx.canceled && _toggleAimMode)
                Fire2Released?.Invoke();
        }

        public void OnReload(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                ReloadButtonPressed?.Invoke();
        }

        public void Tick()
        {
            MovementButtonPressed?.Invoke(_movementVector);
            HandleMouseButton();
            HandleItemSelection();
        }

        private void HandleItemSelection()
        {
            for (int i = 0; i < 10; i++)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    WeaponSelectPressed?.Invoke(i);
                    break;
                }
            }
        }

        private void HandleMouseButton()
        {
            if (UnityEngine.Input.GetMouseButton(0)) 
                Fire1Pressed?.Invoke();

            if (UnityEngine.Input.GetMouseButtonUp(0))
                Fire1Released?.Invoke();
        }
    }
}