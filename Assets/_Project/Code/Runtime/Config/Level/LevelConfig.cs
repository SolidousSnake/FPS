using _Project.Code.Runtime.Config.Gameplay;
using _Project.Code.Runtime.States;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Code.Runtime.Config.Level
{
    [CreateAssetMenu(menuName = "Source/Config/Level", fileName = "New level config")]
    public class LevelConfig : SerializedScriptableObject
    {
        [field: SerializeField] public InitialState InitialState { get; private set; }
        [field: SerializeField] public MusicConfig Music { get; private set; }
        [field: SerializeField] public float AlertStateDuration { get; private set; }
        [field: SerializeField] public float EvasionStateDuration { get; private set; }
     }
}