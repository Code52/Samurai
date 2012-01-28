namespace SamuraiServer.Data.Providers
{
    public interface ICombatCalculator
    {
        double CalculateDamage(Unit attackUnit, Unit targetUnit);
    }
}