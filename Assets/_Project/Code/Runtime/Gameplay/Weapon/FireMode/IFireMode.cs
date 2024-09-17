using System;
using Cysharp.Threading.Tasks;

namespace _Project.Code.Runtime.Weapon.FireMode
{
    public interface IFireMode
    {
        public event Action Fired;
        public event Action Stopped;
        public void Fire();
        public void StopFire();
        public void StopFireWithDelay();
    }
}