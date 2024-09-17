using _Project.Code.Runtime.Data.Settings;
using UnityEngine;

namespace _Project.Code.Runtime.Config.Player
{
    [CreateAssetMenu(menuName = "Source/Settings/Player", fileName = "New player settings")]
    public sealed class PlayerSettings : ScriptableObject
    {
        public MouseSettings MouseSettings;
    }
}