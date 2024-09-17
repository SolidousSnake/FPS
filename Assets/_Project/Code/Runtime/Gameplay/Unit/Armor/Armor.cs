using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Armor
{
    public class Armor : MonoBehaviour
    {
        [SerializeField, ValidateInput(nameof(IsValidArmor), "Armor must be a positive number")]
        private float _maxArmor;

        private ReactiveProperty<float> _currentArmor = new ReactiveProperty<float>();

        public float MaxArmor => _maxArmor;
        public IReadOnlyReactiveProperty<float> CurrentArmor => _currentArmor;
        public IObservable<UniRx.Unit> Depleted => _currentArmor.Where(current => current <= 0).AsUnitObservable();

        private bool IsValidArmor(float armor) => armor > 0;

        private void Start() => _currentArmor.Value = _maxArmor;

        public float ApplyDamage(float damage)
        {
            if (_currentArmor.Value < 0)
                return damage;

            float damageToAbsorb = Mathf.Min(_currentArmor.Value, damage);
            _currentArmor.Value -= damageToAbsorb;
            return damage - damageToAbsorb;
        }

        public void ApplyRepair(float armor)
        {
            if (armor < 0)
                throw new ArgumentException($"Armor value must be positive. Received: {armor}");

            _currentArmor.Value += armor;

            if (_currentArmor.Value > _maxArmor)
                _currentArmor.Value = _maxArmor;
        }
        
        public void ResetArmor() => ApplyRepair(_maxArmor);
    }
}