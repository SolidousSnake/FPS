using _Project.Code.Runtime.Core.States;

namespace _Project.Code.Runtime.Weapon.State
{
    public abstract class GunState : IState
    {
        public abstract void Enter();
        public abstract void Exit();
        public virtual void OnShoot() { }
        public virtual void OnReload() { }
        public virtual void OnAim() { }
    }
}