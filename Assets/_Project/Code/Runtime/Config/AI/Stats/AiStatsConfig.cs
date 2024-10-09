using UnityEngine;

namespace _Project.Code.Runtime.Config.AI.Stats
{
    [CreateAssetMenu(menuName = "Source/Config/AI/Stats", fileName = "New ai stats config")]
    public class AiStatsConfig : ScriptableObject
    {
        [field: SerializeField] public float MaxAttackDistance { get; private set; }
        [field: SerializeField] public float MaxVisionDistance { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
        [field: SerializeField] public float UpdateStateDelay { get; private set; }
    }
}