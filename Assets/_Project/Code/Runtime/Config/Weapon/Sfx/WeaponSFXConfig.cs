using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Code.Runtime.Config.Weapon.Sfx
{
    [CreateAssetMenu(menuName = "Source/Config/Weapon/Sfx/Base", fileName = "New weaponSFX config")]
    public class WeaponSfxConfig : ScriptableObject
    {
        [SerializeField] [AssetSelector] private AudioClip[] _fireClip;
        [SerializeField] [AssetSelector] private AudioClip[] _tailClip;
        [field: SerializeField] [field: AssetSelector] public AudioClip ReloadClip { get; private set; }
        [field: SerializeField] [field: AssetSelector] public AudioClip ReloadEmptyClip { get; private set; }

        public AudioClip[] FireClip => _fireClip;
        public AudioClip[] TailClip => _tailClip;
    }
}