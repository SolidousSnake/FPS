using System;
using _Project.Code.Runtime.Weapon.WeaponAttack;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.Weapon.FireMode
{
    public sealed class SemiAutoFire : IFireMode
    {
        private readonly IWeaponAttack _weaponAttack;
        private readonly WeaponFacade _weaponFacade;
        private readonly float _fireDelay;
        private bool _allowShooting;
        private bool _shot;

        public SemiAutoFire(IWeaponAttack weaponAttack, WeaponFacade weaponFacade, float fireDelay)
        {
            _weaponAttack = weaponAttack;
            _weaponFacade = weaponFacade;
            _fireDelay = fireDelay;
            _allowShooting = true;
        }

        public event Action Fired;
        public event Action Stopped;

        public void Fire()
        {
            if (_weaponFacade.AllowFire) 
                ExecuteFire();
        }

        private void ExecuteFire()
        {
            if (!_allowShooting || _shot)
                return;

            _weaponAttack.Attack();
            Fired?.Invoke();
            _allowShooting = false;
            _shot = true;
            HandleFireDelay().Forget();
        }

        private async UniTask HandleFireDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_fireDelay));
            Stopped?.Invoke();
            _allowShooting = true;
        }

        public void StopFire()
        {
            _shot = false;
        }

        public void StopFireWithDelay()
        {
        }
    }
}