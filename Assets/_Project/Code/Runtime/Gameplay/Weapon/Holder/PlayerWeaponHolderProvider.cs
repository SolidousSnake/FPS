using System;
using _Project.Code.Runtime.Services.Input;
using _Project.Code.Runtime.UI.View;
using _Project.Code.Runtime.UI.View.Crosshair;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.Holder
{
    public class PlayerWeaponHolderProvider : IDisposable
    {
        private readonly InputService _inputService;
        private readonly WeaponHolder _weaponHolder;

        public PlayerWeaponHolderProvider(InputService inputService, WeaponAmmoView weaponAmmoView,
            CrosshairView crosshairView, WeaponHolder weaponHolder)
        {
            _inputService = inputService;
            _weaponHolder = weaponHolder;
            _weaponHolder.Initialize(weaponAmmoView, crosshairView);

            _inputService.Fire1Pressed += _weaponHolder.Attack;
            _inputService.Fire1Released += _weaponHolder.StopAttack;
            _inputService.Fire2Pressed += _weaponHolder.Aim;
            _inputService.Fire2Released += _weaponHolder.Aim;

            _inputService.ReloadButtonPressed += _weaponHolder.Reload;
            _inputService.WeaponSelectPressed += _weaponHolder.SelectWeaponByIndex;

            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Cursor in weapon holder provider");
        }

        public void Dispose()
        {
            _inputService.Fire1Pressed -= _weaponHolder.Attack;
            _inputService.Fire1Released -= _weaponHolder.StopAttack;
            _inputService.Fire2Pressed -= _weaponHolder.Aim;
            _inputService.Fire2Released -= _weaponHolder.Aim;

            _inputService.ReloadButtonPressed -= _weaponHolder.Reload;
            _inputService.WeaponSelectPressed -= _weaponHolder.SelectWeaponByIndex;
        }
    }
}