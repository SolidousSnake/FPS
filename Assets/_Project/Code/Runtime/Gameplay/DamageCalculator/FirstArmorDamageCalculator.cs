using _Project.Code.Runtime.Unit.Armor;
using _Project.Code.Runtime.Unit.Health;

namespace _Project.Code.Runtime.DamageCalculator
{
    public class FirstArmorDamageCalculator : IDamageCalculator
    {
        private readonly Health _health;
        private readonly Armor _armor;

        public FirstArmorDamageCalculator(Health health, Armor armor)
        {
            _health = health;
            _armor = armor;
        }
        
        public void Calculate(float damage)
        {
            float remainingDamage = _armor.ApplyDamage(damage);
            
            if(remainingDamage > 0)
                _health.ApplyDamage(remainingDamage);
        }
    }
}