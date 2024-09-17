using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Code.Runtime.Config.Weapon.Sfx
{
    [CreateAssetMenu(menuName = "Source/Config/Weapon/Sfx/FullAuto", fileName = "New weaponSFX config")]
    public class FullAutoSfxConfig : WeaponSfxConfig
    {
        [SerializeField] [AssetSelector] private AudioClip _oneShotClip;
        
        public AudioClip OneShotClip => _oneShotClip;
    }
}