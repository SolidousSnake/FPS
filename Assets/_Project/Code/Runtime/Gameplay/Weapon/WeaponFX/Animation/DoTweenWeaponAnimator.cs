using System;
using System.Collections.Generic;
using _Project.Code.Runtime.Services.Camera;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Weapon.WeaponFX.Animation
{
    public sealed class DoTweenWeaponAnimator : MonoBehaviour
    {
        [SerializeField] private Vector3 _originalWeaponPosition;
        [SerializeField] private Vector3 _originalWeaponRotation;

        [TabGroup("Aim")] [SerializeField] private Vector3 _aimPosition;
        [TabGroup("Aim")] [SerializeField] private float _aimDuration;
        [TabGroup("Aim")] [SerializeField] private float _fovZoom;

        [TabGroup("Reload")] [SerializeField] private List<AnimationStage> _stages;
        [TabGroup("Reload")] [SerializeField] private List<AnimationStage> _boltStages;

        [TabGroup("Recoil")] [SerializeField] private Vector3 _recoilRotation;
        [TabGroup("Recoil")] [SerializeField] private Ease _recoilEase;
        [TabGroup("Recoil")] [SerializeField] private float _recoilDuration;
        [TabGroup("Recoil")] [SerializeField] private float _recoilCooldown;
        [TabGroup("Recoil")] [SerializeField] private float _recoilStep;

        [TabGroup("Selection")] [SerializeField] private Vector3 _deselectPosition;
        [TabGroup("Selection")] [SerializeField] private Vector3 _deselectRotation;

        [Inject] private CameraService _cameraService;

        private Vector3 _currentWeaponPosition;

        private void OnValidate()
        {
            _currentWeaponPosition = _originalWeaponPosition;
        }

        [Button]
        public void PlaySelect(float duration)
        {
            transform.DOLocalMove(_originalWeaponPosition, duration);
            transform.DOLocalRotate(_originalWeaponRotation, duration);
        }

        [Button]
        public void PlayDeselect(float duration)
        {
            transform.DOLocalMove(_deselectPosition, duration);
            transform.DOLocalRotate(_deselectRotation, duration);
        }

        public void PlayRecoil()
        {
            var recoilSequence = DOTween.Sequence();

            recoilSequence
                .Append(transform.DOLocalMoveZ(_currentWeaponPosition.z - _recoilStep, _recoilDuration)
                    .SetEase(_recoilEase))
                .Join(transform.DOLocalRotate(_originalWeaponRotation + _recoilRotation, _recoilDuration)
                    .SetEase(_recoilEase))
                .SetLink(gameObject);
        }

        public void StopPlayRecoil()
        {
            var recoilSequence = DOTween.Sequence();

            recoilSequence
                .Append(transform.DOLocalMoveZ(_currentWeaponPosition.z, _recoilCooldown)
                    .SetEase(_recoilEase))
                .Join(transform.DOLocalRotate(_originalWeaponRotation, _recoilCooldown)
                    .SetEase(_recoilEase))
                .SetLink(gameObject);
        }

        public void PlayAim(bool aimed)
        {
            if (aimed)
            {
                transform.DOLocalMove(_aimPosition, _aimDuration);
                _cameraService.ZoomIn(_fovZoom, _aimDuration);
                _currentWeaponPosition = _aimPosition;
            }
            else
            {
                transform.DOLocalMove(_originalWeaponPosition, _aimDuration);
                _cameraService.ZoomOut(_aimDuration);
                _currentWeaponPosition = _originalWeaponPosition;
            }
        }

        [Button]
        public void PlayReload(bool empty)
        {
            var reloadSequence = DOTween.Sequence();
            AddStagesToSequence(reloadSequence, _stages);

            if (empty)
                AddStagesToSequence(reloadSequence, _boltStages);
        }

        private void AddStagesToSequence(Sequence sequence, List<AnimationStage> stages)
        {
            foreach (var stage in stages)
            {
                sequence
                    .Append(stage.GameObject.transform.DOLocalMove(stage.Position, stage.Duration).SetEase(stage.Ease))
                    .Join(stage.GameObject.transform.DOLocalRotate(stage.Rotation, stage.Duration).SetEase(stage.Ease));
            }
        }
    }
}