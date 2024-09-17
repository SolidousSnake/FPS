using System;
using _Project.Code.Runtime.UI.View;

namespace _Project.Code.Runtime.Weapon
{
    public sealed class WeaponAmmo
    {
        private readonly WeaponAmmoView _weaponAmmoView;
        
        public WeaponAmmo(int cartridgesInMagazine, int availableCartridges)
        {
            MagazineCapacity = CartridgesInMagazine = cartridgesInMagazine;
            AvailableCartridges = availableCartridges;
        }

        public WeaponAmmo(int cartridgesInMagazine, int availableCartridges, WeaponAmmoView weaponAmmoView)
        {
            MagazineCapacity = CartridgesInMagazine = cartridgesInMagazine;
            AvailableCartridges = availableCartridges;
            _weaponAmmoView = weaponAmmoView;
        }

        public int MagazineCapacity { get; }
        public int CartridgesInMagazine { get; private set; }
        public int AvailableCartridges { get; private set; }
        public bool IsEmpty => CartridgesInMagazine == 0;
        public bool AllowReload => CartridgesInMagazine != MagazineCapacity && AvailableCartridges > 0;

        public event Action BecameEmpty;

        public void Subtract()
        {
            CartridgesInMagazine--;
            if (CartridgesInMagazine <= 0)
            {
                CartridgesInMagazine = 0;
                BecameEmpty?.Invoke();
            }

            UpdateView();
        }

        public void Reload(int value)
        {
            CartridgesInMagazine += value;
            AvailableCartridges -= value;
            UpdateView();
        }

        private void UpdateView() => _weaponAmmoView?.SetAmount(CartridgesInMagazine, AvailableCartridges);
    }
}