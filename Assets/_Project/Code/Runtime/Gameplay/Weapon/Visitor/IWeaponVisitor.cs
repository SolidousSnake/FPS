using UnityEngine;

namespace _Project.Code.Runtime.Weapon.Visitor
{
    public interface IWeaponVisitor
    {
        public void Visit(Vector3 point, Vector3 normal, float damage);
    }
}