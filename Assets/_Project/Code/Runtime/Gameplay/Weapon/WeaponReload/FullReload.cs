using System;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.Weapon.WeaponReload
{
    public sealed class FullReload : IWeaponReload
    {
        private readonly WeaponAmmo _weaponAmmo;
        private readonly float _duration;
        public bool IsReloading { get; private set; }

        public FullReload(WeaponAmmo weaponAmmo, float duration)
        {
            _weaponAmmo = weaponAmmo;
            _duration = duration;
        }
        
        public event Action<bool> ReloadingStarted;

        public async UniTask Reload()
        {
            if(IsReloading || !_weaponAmmo.AllowReload)
                return;

            
            IsReloading = true;
            ReloadingStarted?.Invoke(_weaponAmmo.IsEmpty);

            int reason = Math.Min(_weaponAmmo.MagazineCapacity - _weaponAmmo.CartridgesInMagazine, _weaponAmmo.AvailableCartridges);

            await UniTask.WaitForSeconds(_duration);
            _weaponAmmo.Reload(reason);
            IsReloading = false;
        }
    }
}