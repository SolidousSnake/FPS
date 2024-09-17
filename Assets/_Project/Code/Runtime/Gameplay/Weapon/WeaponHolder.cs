using System.Linq;
using _Project.Code.Runtime.Services.Input;
using _Project.Code.Runtime.UI.View;
using _Project.Code.Runtime.UI.View.Crosshair;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Weapon
{
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private WeaponFacade[] _weapons;

        private InputService _inputService;
        private int _selectedWeaponIndex;
        
        [Inject]
        private void Construct(InputService inputService, WeaponAmmoView weaponAmmoView, CrosshairView crosshairView)
        {
            _inputService = inputService;
            
            _inputService.Fire1Pressed += Attack;
            _inputService.Fire1Released += StopAttack;
            _inputService.Fire2Pressed += Aim;
            _inputService.Fire2Released += Aim;

            _inputService.ReloadButtonPressed += Reload;
            _inputService.WeaponSelectPressed += SelectWeaponByIndex;
            
            InitializeWeapons(weaponAmmoView, crosshairView);
            SelectWeaponByIndex(1);
            
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Cursor in weapon holder");
        }
        
        private void InitializeWeapons(WeaponAmmoView weaponAmmoView, CrosshairView crosshairView)
        {
            foreach (var weapon in _weapons)
                weapon.Initialize(weaponAmmoView, crosshairView);
        }

        private void Attack() => _weapons[_selectedWeaponIndex].Shoot();
        private void StopAttack() => _weapons[_selectedWeaponIndex].StopShoot();
        private void Reload() => _weapons[_selectedWeaponIndex].Reload();
        private void Aim() => _weapons[_selectedWeaponIndex].Aim();

        private void SelectWeaponByIndex(int index)
        {
            int adjustedIndex = index - 1;

            if (adjustedIndex >= 0 && adjustedIndex < _weapons.Length)
            {
                _selectedWeaponIndex = adjustedIndex;
                Switch().Forget();
            }
        }

        private async UniTaskVoid Switch()
        {
            if (_weapons[_selectedWeaponIndex].IsSelected)
                return;

            var deselectTasks = Enumerable.Select(_weapons, t => t.Deselect()).ToList();
            await UniTask.WhenAll(deselectTasks);
            await _weapons[_selectedWeaponIndex].Select();
        }
    }
}
