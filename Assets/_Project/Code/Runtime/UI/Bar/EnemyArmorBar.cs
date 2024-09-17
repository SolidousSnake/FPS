using System;
using _Project.Code.Runtime.Unit.Armor;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Code.Runtime.UI.Bar
{
    public class EnemyArmorBar : MonoBehaviour
    {
        [SerializeField] private Image _armorFill;
        [SerializeField] private Image _delayedArmorFill;
        [SerializeField] private float _updateDelay;
        [SerializeField] private float _fillSpeed;

        private Armor _armor;

        public void Initialize(Armor armor)
        {
            _armor = armor;
            _armor.CurrentArmor.Subscribe(SetArmorAmount).AddTo(this);
            _armor.CurrentArmor.Throttle(TimeSpan.FromSeconds(_updateDelay))
                .Subscribe(SetDelayedHealthAmount).AddTo(this);
        }

        private void SetArmorAmount(float amount) => _armorFill.fillAmount = amount / _armor.MaxArmor;
        
        private void SetDelayedHealthAmount(float amount) =>
            _delayedArmorFill.DOFillAmount(amount / _armor.MaxArmor, _fillSpeed);
        
        [Sirenix.OdinInspector.Button]
        private void DamageArmor() => 
            _armor.ApplyDamage(Mathf.Max(0, _armor.CurrentArmor.Value - UnityEngine.Random.Range(5, 30)));

        [Sirenix.OdinInspector.Button]
        private void Reset() => _armor.ResetArmor();
    }
}