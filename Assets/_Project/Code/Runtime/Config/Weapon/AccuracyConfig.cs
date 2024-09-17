using UnityEngine;

namespace _Project.Code.Runtime.Config.Weapon
{
    [CreateAssetMenu(menuName = "Source/Config/Weapon/Accuracy", fileName = "New accuracy config")]
    public sealed class AccuracyConfig : ScriptableObject
    {
        [SerializeField] private float _maxAimSpreadFactor;
        [SerializeField] private float _minAimSpreadFactor;
        [Space]
        [SerializeField] private float _maxHipSpreadFactor;
        [SerializeField] private float _minHipSpreadFactor;
        [Space]
        [SerializeField] private float _spreadStep;
        [SerializeField] private float _spreadRecoverySpeed;
        
        public float MaxAimSpreadFactor => _maxAimSpreadFactor;
        public float MinAimSpreadFactor => _minAimSpreadFactor;
        public float MaxHipSpreadFactor => _maxHipSpreadFactor;
        public float MinHipSpreadFactor => _minHipSpreadFactor;
        public float SpreadStep => _spreadStep;
        public float SpreadRecoverySpeed => _spreadRecoverySpeed;
    }
}