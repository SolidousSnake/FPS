using System;
using _Project.Code.Runtime.UI.View.Crosshair;

namespace _Project.Code.Runtime.Weapon.Aim
{
    public class WeaponAim
    {
        private bool _aiming = false;

        public event Action<bool> Aimed;
        
        public void Aim()
        {
            _aiming = !_aiming;
            Aimed?.Invoke(_aiming);
        }
    }
}