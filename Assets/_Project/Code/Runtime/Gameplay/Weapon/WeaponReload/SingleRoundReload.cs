using System;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.Weapon.WeaponReload
{
    public sealed class SingleRoundReload : IWeaponReload
    {
        private readonly WeaponAmmo _weaponAmmo;
        private readonly float _duration;
        public bool IsReloading { get; private set; }

        public SingleRoundReload(WeaponAmmo weaponAmmo, float duration)
        {
            _weaponAmmo = weaponAmmo;
            _duration = duration;
        }

        public event Action<bool> ReloadingStarted;

        public async UniTask Reload()
        {
            if (IsReloading || _weaponAmmo.CartridgesInMagazine == _weaponAmmo.MagazineCapacity)
                return;

            IsReloading = true;
            
            int reason = Math.Min(_weaponAmmo.MagazineCapacity - _weaponAmmo.CartridgesInMagazine
                , _weaponAmmo.AvailableCartridges);

            for (int i = 0; i < reason; i++)
            {
                ReloadingStarted?.Invoke(_weaponAmmo.IsEmpty);
                await UniTask.WaitForSeconds(_duration);
                _weaponAmmo.Reload(1);
            }

            IsReloading = false;
        }
    }
}