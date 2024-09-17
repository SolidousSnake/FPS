using System;
using _Project.Code.Runtime.Weapon.Visitor;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.Projectile
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private ForceMode _forceMode;
        [SerializeField, Min(0f)] private float _force;
        [SerializeField, Min(0f)] private float _lifeTime;

        private float _damage;
        
        private void OnValidate()
        {
            _rigidBody ??= GetComponent<Rigidbody>();
        }

        public void Initialize(Vector3 direction, float damage)
        {
            if (damage <= 0 || direction == Vector3.zero)
                throw new ArgumentException($"Something wrong. Check arguments: Direction {direction}, damage {damage}");
            
            _damage = damage;
            _rigidBody.AddForce(direction.normalized * _force, _forceMode);
            Destroy(gameObject, _lifeTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            ContactPoint contact = other.GetContact(0);
            
            if(other.collider.TryGetComponent(out IWeaponVisitor visitor))
                visitor.Visit(contact.point, contact.normal, _damage);
            
            Destroy(gameObject);
        }
    }
}