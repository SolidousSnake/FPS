using UnityEngine;

namespace _Project.Code.Runtime.Config.Weapon
{
    [CreateAssetMenu(menuName = "Source/Config/Weapon/Recoil", fileName = "New recoil config")]
    public sealed class RecoilConfig : ScriptableObject
    {
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _recoilDuration;
        [SerializeField] private float _recoilCooldown;

        public Vector3 Direction => _direction;
        public float Duration => _recoilDuration;
        public float Cooldown => _recoilCooldown;
    }
}