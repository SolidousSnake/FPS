using System;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.Weapon.WeaponReload
{
    public interface IWeaponReload
    {
        public event Action<bool> ReloadingStarted;
        public bool IsReloading { get; }
        public UniTask Reload();
    }
}