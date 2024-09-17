using UnityEngine;

namespace _Project.Code.Runtime.Weapon.WeaponFX
{
    public sealed class WeaponVfx
    {
        private readonly ParticleSystem _muzzleFlash;
        private readonly Transform _muzzle;
        
        public WeaponVfx(Transform muzzle, ParticleSystem muzzleFlash)
        {
            _muzzle = muzzle;
            _muzzleFlash = muzzleFlash;
        }

        public void PlayMuzzleFlash() => Object.Instantiate(_muzzleFlash, _muzzle.position, _muzzle.rotation);
    }
}