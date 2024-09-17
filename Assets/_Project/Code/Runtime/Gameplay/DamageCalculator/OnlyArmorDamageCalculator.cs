using _Project.Code.Runtime.Unit.Armor;

namespace _Project.Code.Runtime.DamageCalculator
{
    public class OnlyArmorDamageCalculator : IDamageCalculator
    {
        private readonly Armor _armor;

        public OnlyArmorDamageCalculator(Armor armor)
        {
            _armor = armor;
        }

        public void Calculate(float damage)
        {
            if(damage > 0)
                _armor.ApplyDamage(damage);
        }
    }
}