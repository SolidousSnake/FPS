using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Code.Runtime.Config.AI.Quotes
{
    [CreateAssetMenu(menuName = "Source/Config/AI/Quotes", fileName = "New quotes config")]
    public class QuotesConfig : ScriptableObject
    {
        [field: SerializeField] [field: AssetSelector] public AudioClip[] SearchClips { get; private set; }
        [field: SerializeField] [field: AssetSelector] public AudioClip[] TargetSightedClips { get; private set; }
        [field: SerializeField] [field: AssetSelector] public AudioClip[] ChasingClips { get; private set; }
        [field: SerializeField] [field: AssetSelector] public AudioClip[] DuringCombatClips { get; private set; }
        [field: SerializeField] [field: AssetSelector] public AudioClip[] DeathClips { get; private set; }
    }
}