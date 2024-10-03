using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Code.Runtime.Config.Gameplay
{
    [CreateAssetMenu(menuName = "Source/Config/Gameplay/Music", fileName = "New music config")]
    public class MusicConfig : ScriptableObject
    {
        [field: Title("Stealth")]
        [field: SerializeField]
        public AudioClip StealthLoopClip { get; private set; }

        [field: Title("Evasion")]
        [field: SerializeField]
        public AudioClip EvasionLoopClip { get; private set; }

        [field: Title("Alert")]
        [field: SerializeField]
        public AudioClip AlertIntroClip { get; private set; }

        [field: SerializeField]
        public AudioClip AlertLoopClip { get; private set; }
    }
}