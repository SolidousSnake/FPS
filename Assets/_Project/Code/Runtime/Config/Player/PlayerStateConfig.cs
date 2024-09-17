using UnityEngine;

namespace _Project.Code.Runtime.Config.Player
{
   [CreateAssetMenu(menuName = "Source/Config/Player/PlayerState", fileName = "New player state")]
    public sealed class PlayerStateConfig : ScriptableObject
    {
        public float MovementSpeed;
        public float ColliderHeight;
        public Vector3 ColliderCenter;
        public Vector3 CameraPosition;
    }
}