using _Project.Code.Runtime.Core.Utils;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.WeaponFX.Animation
{
    public sealed class WeaponAnimator
    {
        private readonly int _walkHash = Animator.StringToHash(Constants.Animation.Weapon.Walk);
        private readonly int _runHash = Animator.StringToHash(Constants.Animation.Weapon.Run);
        private readonly int _aimHash = Animator.StringToHash(Constants.Animation.Weapon.Aim);
        private readonly int _fireHash = Animator.StringToHash(Constants.Animation.Weapon.Fire);
        private readonly int _reloadHash = Animator.StringToHash(Constants.Animation.Weapon.Reload);
        private readonly int _emptyMagazineHash = Animator.StringToHash(Constants.Animation.Weapon.EmptyMagazine);
        private readonly int _selectHash = Animator.StringToHash(Constants.Animation.Weapon.Select);
        private readonly int _deselectHash = Animator.StringToHash(Constants.Animation.Weapon.Deselect);

        private readonly Animator _animator;

        public WeaponAnimator(Animator animator)
        {
            _animator = animator;
        }

        public void PlayReload(bool isEmpty)
        {
            _animator.SetBool(_emptyMagazineHash, isEmpty);
            _animator.SetTrigger(_reloadHash);
        }

        public void PlayFire() => _animator.SetBool(_fireHash, true);
        public void StopFire() => _animator.SetBool(_fireHash, false);
        public void PlayAim(bool value) => _animator.SetBool(_aimHash, value);
        public void PlaySelect() => _animator.SetTrigger(_selectHash);
        public void PlayDeselect() => _animator.SetTrigger(_deselectHash);
    }
}