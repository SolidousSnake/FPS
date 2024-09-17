using System;
using _Project.Code.Runtime.Config.Weapon.Sfx;
using _Project.Code.Runtime.Weapon.FireMode;
using _Project.Code.Runtime.Weapon.Projectile;
using _Project.Code.Runtime.Weapon.WeaponAttack;
using _Project.Code.Runtime.Weapon.WeaponReload;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Code.Runtime.Config.Weapon
{
    [CreateAssetMenu(menuName = "Source/Config/Weapon/Base", fileName = "New weapon config")]
    public sealed class WeaponConfig : ScriptableObject
    {
        [TabGroup("Config")] [AssetSelector] [SerializeField]
        private WeaponSfxConfig _sfxConfig;

        [TabGroup("Config")] [AssetSelector] [SerializeField]
        private AccuracyConfig _accuracyConfig;

        [TabGroup("Config")] [AssetSelector] [SerializeField]
        private RecoilConfig _recoilConfig;

        [SerializeField] private Projectile _projectilePrefab;

        [Header("Type")] [EnumToggleButtons] [SerializeField]
        private AttackType _attackType;

        [EnumToggleButtons] [SerializeField] private ReloadType _reloadType;
        [EnumToggleButtons] [SerializeField] private FireModeType _fireModeType;

        [TabGroup("Ammo")] [SerializeField] private AmmoType _ammoType;

        [field: TabGroup("Ammo")]
        [field: ShowIf(nameof(_ammoType), AmmoType.Pellets)]
        [field: SerializeField]
        public int Pellets { get; private set; }

        [TabGroup("Ammo")] [SerializeField] private int _cartridgesInMagazine;
        [TabGroup("Ammo")] [SerializeField] private int _availableCartridges;
        [TabGroup("Ammo")] [SerializeField] private float _reloadDuration;

        [TabGroup("Switching")] [SerializeField]
        private float _selectTime;

        [TabGroup("Switching")] [SerializeField]
        private float _deselectTime;

        [TabGroup("Combat")] [SerializeField] private float _fireDelay;
        [TabGroup("Combat")] [SerializeField] private float _damage;
        // [field: TabGroup("Combat")] [field: SerializeField] public float HeadDamageMultiplier { get; private set; }
        // [field: TabGroup("Combat")] [field: SerializeField] public float BodyDamageMultiplier { get; private set; }
        // [field: TabGroup("Combat")] [field: SerializeField] public float ArmDamageMultiplier { get; private set; }
        // [field: TabGroup("Combat")] [field: SerializeField] public float LegDamageMultiplier { get; private set; }

        [TabGroup("Aim")] [SerializeField] private float _zoomInSpeed;
        [TabGroup("Aim")] [SerializeField] private float _zoomOutSpeed;
        [TabGroup("Aim")] [SerializeField] private float _fovZoom;

        public WeaponSfxConfig Sfx => _sfxConfig;
        public AccuracyConfig AccuracyConfig => _accuracyConfig;
        public RecoilConfig RecoilConfig => _recoilConfig;
        public Projectile ProjectilePrefab => _projectilePrefab;

        public AttackType AttackType => _attackType;
        public ReloadType ReloadType => _reloadType;
        public FireModeType FireMode => _fireModeType;

        public int CartridgesInMagazine => _cartridgesInMagazine;
        public int AvailableCartridges => _availableCartridges;
        public float ReloadDuration => _reloadDuration;

        public float SelectTime => _selectTime;
        public float DeselectTime => _deselectTime;

        public float FireDelay => _fireDelay;
        public float Damage => _damage;

        public float ZoomInSpeed => _zoomInSpeed;
        public float ZoomOutSpeed => _zoomOutSpeed;
        public float FovZoom => _fovZoom;

        private void OnValidate()
        {
            if (_ammoType == AmmoType.Slug)
                Pellets = 1;
        }
    }
}