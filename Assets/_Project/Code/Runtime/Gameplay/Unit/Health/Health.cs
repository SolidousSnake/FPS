using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace _Project.Code.Runtime.Unit.Health
{
    public class Health : MonoBehaviour
    {
        [SerializeField, ValidateInput(nameof(IsValidHealth), "Health must be a positive number")]
        private float _maxHealth;
        private ReactiveProperty<float> _currentHealth = new ReactiveProperty<float>();

        public float MaxHealth => _maxHealth;
        public IReadOnlyReactiveProperty<float> CurrentHealth => _currentHealth;
        public IObservable<UniRx.Unit> Depleted => _currentHealth.Where(current => current <= 0).AsUnitObservable();

        private bool IsValidHealth(float health) => health > 0;
        private void Start() => _currentHealth.Value = _maxHealth;

        public void ApplyDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentException($"Damage value must be positive. Received: {damage}");

            _currentHealth.Value -= damage;

            if (_currentHealth.Value <= 0)
                _currentHealth.Value = 0;
        }

        public void ApplyHeal(float health)
        {
            if (health < 0)
                throw new ArgumentException($"Healing value must be positive. Received: {health}");

            _currentHealth.Value += health;

            if (_currentHealth.Value > _maxHealth)
                _currentHealth.Value = _maxHealth;
        }

        public void ResetHealth() => ApplyHeal(_maxHealth);
    }
}