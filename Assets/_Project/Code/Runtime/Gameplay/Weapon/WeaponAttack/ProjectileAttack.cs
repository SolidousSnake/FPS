using _Project.Code.Runtime.Config.Weapon;
using UnityEngine;

namespace _Project.Code.Runtime.Weapon.WeaponAttack
{
    public sealed class ProjectileAttack : IWeaponAttack
    {
        private readonly Projectile.Projectile _projectile;
        private readonly WeaponConfig _config;
        private readonly Transform _shootPoint;
        private readonly Spread _spread;
        private readonly int _attackCount;

        public ProjectileAttack(Transform shootPoint, WeaponConfig config, Spread spread, int attackCount)
        {
            _projectile = config.ProjectilePrefab;
            _shootPoint = shootPoint;
            _config = config;
            _spread = spread;
            _attackCount = attackCount;
        }

        public void Attack()
        {
            for (int i = 0; i < _attackCount; i++)
            {
                var projectile = Object.Instantiate(_projectile, _shootPoint.position, _shootPoint.rotation);
                projectile.Initialize(_shootPoint.forward + _spread.Get(), _config.Damage);
            }
            
            _spread.IncreaseSpread();
            _spread.StartResetSpreadTimer();
        }
    }
}