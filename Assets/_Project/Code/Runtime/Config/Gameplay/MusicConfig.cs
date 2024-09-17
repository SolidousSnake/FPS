using UnityEngine;

namespace _Project.Code.Runtime.Config.Gameplay
{
    [CreateAssetMenu(menuName = "Source/Config/Gameplay/Music", fileName = "New music config")]
    public class MusicConfig : ScriptableObject
    {
        [field: SerializeField] public AudioClip IdleLoopClip { get; private set; }
        [field: Header("Alert")]
        [field: SerializeField] public AudioClip AlertIntroClip { get; private set; }
        [field: SerializeField] public AudioClip AlertLoopClip { get; private set; }
    }
}