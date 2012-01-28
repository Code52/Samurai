using System;

namespace SamuraiServer.Data.Providers
{
    public class CombatCalculator : ICombatCalculator
    {
        public double CalculateDamage(Unit attackUnit, Unit targetUnit)
        {
            return Math.Pow(attackUnit.Attack + 1, 1.5) / Math.Pow(targetUnit.Defence + 1, 1.3) * 10;
        }
    }
}
