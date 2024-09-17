using _Project.Code.Runtime.Config.Gameplay;
using _Project.Code.Runtime.States;
using UnityEngine;

namespace _Project.Code.Runtime.Config.Level
{
    [CreateAssetMenu(menuName = "Source/Config/Level", fileName = "New level config")]
    public class GameplaySceneConfig : ScriptableObject
    {
        [field: SerializeField] public InitialState InitialState { get; private set; }
        [field: SerializeField] public MusicConfig Music { get; private set; }
    }
}