using System.Linq;
using _Project.Code.Runtime.UI.View;
using _Project.Code.Runtime.UI.View.Crosshair;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.Holder
{
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private WeaponFacade[] _weapons;

        private int _selectedWeaponIndex;
        
        public void Initialize(WeaponAmmoView weaponAmmoView, CrosshairView crosshairView)
        {
            InitializeWeapons(weaponAmmoView, crosshairView);
            SelectWeaponByIndex(1);
            
        }
        
        private void InitializeWeapons(WeaponAmmoView weaponAmmoView, CrosshairView crosshairView)
        {
            foreach (var weapon in _weapons)
                weapon.Initialize(weaponAmmoView, crosshairView);
        }

        public void Attack() => _weapons[_selectedWeaponIndex].Shoot();
        public void StopAttack() => _weapons[_selectedWeaponIndex].StopShoot();
        public void Reload() => _weapons[_selectedWeaponIndex].Reload();
        public void Aim() => _weapons[_selectedWeaponIndex].Aim();

        public void SelectWeaponByIndex(int index)
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
