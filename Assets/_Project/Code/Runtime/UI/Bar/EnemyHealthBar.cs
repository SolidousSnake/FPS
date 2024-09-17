using System;
using _Project.Code.Runtime.Unit.Health;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Code.Runtime.UI.Bar
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Image _healthFill;
        [SerializeField] private Image _delayedHealthFill;
        [SerializeField] private float _updateDelay;
        [SerializeField] private float _fillSpeed;

        private Health _health;

        public void Initialize(Health health)
        {
            _health = health;
            _health.CurrentHealth.Subscribe(SetHealthAmount).AddTo(this);
            _health.CurrentHealth.Throttle(TimeSpan.FromSeconds(_updateDelay))
                .Subscribe(SetDelayedHealthAmount).AddTo(this);
        }

        private void SetHealthAmount(float amount) => _healthFill.fillAmount = amount / _health.MaxHealth;

        private void SetDelayedHealthAmount(float amount) =>
            _delayedHealthFill.DOFillAmount(amount / _health.MaxHealth, _fillSpeed);
        
        [Sirenix.OdinInspector.Button]
        private void DamageHealth() => 
            _health.ApplyDamage(Mathf.Max(0, _health.CurrentHealth.Value - UnityEngine.Random.Range(5, 30)));

        [Sirenix.OdinInspector.Button]
        private void Reset() => _health.ResetHealth();
    }
}