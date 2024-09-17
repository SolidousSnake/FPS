using System;
using _Project.Code.Runtime.Config.Weapon;
using _Project.Code.Runtime.UI.View;
using _Project.Code.Runtime.UI.View.Crosshair;
using _Project.Code.Runtime.Weapon.Aim;
using _Project.Code.Runtime.Weapon.FireMode;
using _Project.Code.Runtime.Weapon.Recoil;
using _Project.Code.Runtime.Weapon.WeaponAttack;
using _Project.Code.Runtime.Weapon.WeaponFX;
using _Project.Code.Runtime.Weapon.WeaponFX.Animation;
using _Project.Code.Runtime.Weapon.WeaponFX.Sfx;
using _Project.Code.Runtime.Weapon.WeaponReload;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon
{
    public class WeaponFacade : MonoBehaviour
    {
        [SerializeField] private WeaponConfig _config;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private Transform _muzzleView;
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private AudioSource _fireAudioSource;
        [SerializeField] private AudioSource _tailAudioSource;
        [SerializeField] private DoTweenWeaponAnimator _animator;
        [SerializeField] private Transform _head;

        private IWeaponAttack _weaponAttack;
        private IWeaponReload _weaponReload;
        private IFireMode _fireMode;

        private WeaponAim _aim;
        private WeaponAmmo _ammo;
        private WeaponSfx _sfx;
        private WeaponVfx _vfx;
        private Spread _spread;
        private WeaponRecoil _recoil;

        private WeaponAmmoView _ammoView;
        private CrosshairView _crosshairView;

        public bool IsSelected { get; private set; }
        public bool AllowFire => !_ammo.IsEmpty && !_weaponReload.IsReloading && IsSelected;

        public void Initialize(WeaponAmmoView weaponAmmoView = null, CrosshairView crosshairView = null)
        {
            _ammoView = weaponAmmoView;
            _crosshairView = crosshairView;

            _aim = new WeaponAim();
            _ammo = new WeaponAmmo(_config.CartridgesInMagazine, _config.AvailableCartridges, _ammoView);
            _sfx = new WeaponSfx(_fireAudioSource, _tailAudioSource, _config.Sfx);
            _vfx = new WeaponVfx(_muzzleView, _muzzleFlash);
            _spread = new Spread(_config.AccuracyConfig);
            _recoil = new WeaponRecoil(_config.RecoilConfig, _head);

            InstallWeaponAttack();
            InstallWeaponReload();
            InstallWeaponFireMode();
        }

        public void Shoot() => _fireMode.Fire();
        public void StopShoot() => _fireMode.StopFire();
        public void Reload() => _weaponReload.Reload();
        public void Aim() => _aim.Aim();

        public async UniTask Select()
        {
            Subscribe();
            _ammoView.SetAmount(_ammo.CartridgesInMagazine, _ammo.AvailableCartridges);
            gameObject.SetActive(true);
            _animator.PlaySelect(_config.SelectTime);
            _crosshairView.SetAmount(_config.AccuracyConfig.MinHipSpreadFactor, _config.SelectTime);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.SelectTime));
            IsSelected = true;
            OnAimed(false);
        }

        public async UniTask Deselect()
        {
            IsSelected = false;
            _animator.PlayDeselect(_config.DeselectTime);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.DeselectTime));

            Unsubscribe();
            gameObject.SetActive(false);
        }

        private void Subscribe()
        {
            _fireMode.Fired += OnFired;
            _fireMode.Stopped += OnStopFire;

            _aim.Aimed += OnAimed;
            _spread.Changed += _crosshairView.SetAmount;
            _weaponReload.ReloadingStarted += OnReloadStarted;
            _ammo.BecameEmpty += _fireMode.StopFireWithDelay;
        }

        private void Unsubscribe()
        {
            _fireMode.Fired -= OnFired;
            _fireMode.Stopped -= OnStopFire;

            _aim.Aimed -= OnAimed;
            _spread.Changed -= _crosshairView.SetAmount;
            _weaponReload.ReloadingStarted -= OnReloadStarted;
            _ammo.BecameEmpty -= _fireMode.StopFireWithDelay;
        }

        private void OnAimed(bool value)
        {
            _crosshairView.SetVisibility(!value);
            _animator.PlayAim(value);
            _spread.ChangeSpreadFactor(value);
        }

        private void OnReloadStarted(bool value)
        {
            _animator.PlayReload(value);
            _sfx.PlayReload(value);
        }

        private void OnFired()
        {
            _animator.PlayRecoil();
            _ammo.Subtract();
            _sfx.PlayFire();
            _vfx.PlayMuzzleFlash();
            _recoil.ApplyRecoil();
        }

        private void OnStopFire()
        {
            _animator.StopPlayRecoil();
            _sfx.StopFire();
        }

        private void InstallWeaponFireMode()
        {
            _fireMode = _config.FireMode switch
            {
                FireModeType.Full => new FullAutoFire(_weaponAttack, this, _config.FireDelay),
                // FireModeType.Burst => new BurstFire(),
                FireModeType.Semi => new SemiAutoFire(_weaponAttack, this, _config.FireDelay),
                // FireModeType.Gauge => new GaugeAction(),
                _ => throw new ArgumentException("Invalid FireMode type")
            };
        }

        private void InstallWeaponReload()
        {
            _weaponReload = _config.ReloadType switch
            {
                ReloadType.Full => new FullReload(_ammo, _config.ReloadDuration),
                ReloadType.Single => new SingleRoundReload(_ammo, _config.ReloadDuration),
                _ => throw new ArgumentException("Invalid Reload type")
            };
        }

        private void InstallWeaponAttack()
        {
            _weaponAttack = _config.AttackType switch
            {
                AttackType.Projectile => new ProjectileAttack(_shootPoint, _config, _spread, _config.Pellets),
                AttackType.Raycast => throw new Exception("Implement rays"),
                _ => throw new ArgumentException("Invalid Attack type")
            };
        }
    }
}