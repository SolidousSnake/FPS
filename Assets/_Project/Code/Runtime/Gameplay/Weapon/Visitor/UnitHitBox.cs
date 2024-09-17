using _Project.Code.Runtime.DamageCalculator;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.Visitor
{
    public class UnitHitBox : MonoBehaviour, IWeaponVisitor
    {
        [SerializeField] private Collider _hitBox;
        [SerializeField, Min(0.1f)] private float _damageMultiplier;

        private IDamageCalculator _damageCalculator;
        
        public void Initialize(IDamageCalculator damageCalculator) => _damageCalculator = damageCalculator;

        public void Visit(Vector3 point, Vector3 normal, float damage) => 
            _damageCalculator.Calculate(damage * _damageMultiplier);
    }
}