using _Project.Code.Runtime.Config.Weapon;
using _Project.Code.Runtime.Services.Camera;
using DG.Tweening;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.Recoil
{
    public sealed class WeaponRecoil
    {
        private readonly RecoilConfig _config;
        private readonly Transform _context;
        private readonly Quaternion _originalRotation;

        public WeaponRecoil(RecoilConfig config, CameraService cameraService)
        {
            _config = config;
            _context = cameraService.CameraTransform;
            _originalRotation = _context.localRotation;
        }
        public WeaponRecoil(RecoilConfig config, Transform cameraService)
        {
            _config = config;
            _context = cameraService;
            _originalRotation = _context.localRotation;
        }
        
        public void ApplyRecoil()
        {
            var finalRotation = _originalRotation * Quaternion.Euler(_config.Direction);

            _context.DOLocalRotate(finalRotation.eulerAngles, _config.Duration)
                .OnComplete(() => _context.DOLocalRotate(_originalRotation.eulerAngles, _config.Cooldown));
        }
    }
}