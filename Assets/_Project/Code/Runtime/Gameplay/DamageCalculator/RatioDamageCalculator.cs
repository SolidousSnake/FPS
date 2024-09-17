using _Project.Code.Runtime.Unit.Armor;
using _Project.Code.Runtime.Unit.Health;

namespace _Project.Code.Runtime.DamageCalculator
{
    public class RatioDamageCalculator : IDamageCalculator
    {
        private readonly Health _health;
        private readonly Armor _armor;
        private readonly float _armorPercentage;
        private readonly float _healthPercentage;

        public RatioDamageCalculator(Health health, Armor armor, float armorPercentage = .8f, float healthPercentage = .2f)
        {
            _health = health;
            _armor = armor;
            _armorPercentage = armorPercentage;
            _healthPercentage = healthPercentage;
        }
        
        public void Calculate(float damage)
        {
            float damageToArmor = damage * _armorPercentage;
            float damageToHealth = damage * _healthPercentage;
            float remainingDamage = _armor.ApplyDamage(damageToArmor);

            if (remainingDamage > 0) 
                damageToHealth += remainingDamage;

            _health.ApplyDamage(damageToHealth);
        }
    }
}