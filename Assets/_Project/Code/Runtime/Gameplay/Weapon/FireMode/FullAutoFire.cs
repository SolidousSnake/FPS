using System;
using System.Threading;
using _Project.Code.Runtime.Weapon.WeaponAttack;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.FireMode
{
    public sealed class FullAutoFire : IFireMode
    {
        private readonly IWeaponAttack _weaponAttack;
        private readonly WeaponFacade _weaponFacade;
        private readonly float _fireDelay;

        private bool _allowShooting;
        private CancellationTokenSource _cts;

        public FullAutoFire(IWeaponAttack weaponAttack, WeaponFacade weaponFacade, float fireDelay)
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
            _cts = new CancellationTokenSource();

            if (_weaponFacade.AllowFire && _allowShooting)
                ExecuteFire().Forget();
        }

        public void StopFire()
        {
            _cts.Cancel();
            Stopped?.Invoke();
            _allowShooting = true;
        }

        public async void StopFireWithDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_fireDelay));
            StopFire();
        }

        private async UniTask ExecuteFire()
        {
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    _weaponAttack.Attack();
                    Fired?.Invoke();
                    _allowShooting = false;
                    await UniTask.Delay(TimeSpan.FromSeconds(_fireDelay), cancellationToken: _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Stopped?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred during firing: {ex.Message}");
            }
        }
    }
}