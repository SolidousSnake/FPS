using UnityEngine;

namespace _Project.Code.Runtime.Config.Weapon.Sfx
{
    [CreateAssetMenu(menuName = "Source/Config/Weapon/Sfx/Gauge", fileName = "New weaponSFX config")]
    public class GaugeActionSfxConfig : WeaponSfxConfig
    {
        [field: SerializeField] public AudioClip GaugeAction { get; private set; }
    }
}