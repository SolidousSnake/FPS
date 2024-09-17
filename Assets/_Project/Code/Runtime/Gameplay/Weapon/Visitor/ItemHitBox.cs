using UnityEngine;

namespace _Project.Code.Runtime.Weapon.Visitor
{
    public class ItemHitBox : MonoBehaviour, IWeaponVisitor
    {
        [SerializeField] private ParticleSystem _bulletImpact;
        
        public void Visit(Vector3 point, Vector3 normal, float damage)
        {
            var particles = Instantiate(_bulletImpact);
            particles.transform.position = point;
            particles.transform.forward = normal;
        }
    }
}