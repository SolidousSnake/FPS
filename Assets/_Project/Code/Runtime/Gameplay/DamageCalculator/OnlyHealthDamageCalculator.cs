using _Project.Code.Runtime.Unit.Health;

namespace _Project.Code.Runtime.DamageCalculator
{
    public class OnlyHealthDamageCalculator : IDamageCalculator
    {
        private readonly Health _health;

        public OnlyHealthDamageCalculator(Health health)
        {
            _health = health;
        }
        
        public void Calculate(float damage)
        {
            if(damage > 0)
                _health.ApplyDamage(damage);
        }
    }
}