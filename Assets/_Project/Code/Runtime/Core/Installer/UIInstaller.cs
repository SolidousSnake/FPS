using _Project.Code.Runtime.UI.View;
using _Project.Code.Runtime.UI.View.Crosshair;
using UnityEngine;
using Zenject;

namespace _Project.Code.Runtime.Core.Installer
{
    public sealed class UIInstaller : MonoInstaller
    {
        [SerializeField] private WeaponAmmoView _weaponAmmoView;
        [SerializeField] private CrosshairView _crosshairView;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_weaponAmmoView);
            Container.BindInstance(_crosshairView);
        }
    }
}