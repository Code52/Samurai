using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class CombatCalculatorTests
    {
        public class Robot : Unit { }

        public class When_A_Unit_Attacks_A_Similar_Unit : SpecificationFor<CombatCalculator>
        {
            private Robot targetUnit;
            private Robot attackUnit;
            private double result;

            public override CombatCalculator Given()
            {
                attackUnit = new Robot { HitPoints = 1.0, CurrentHitPoints = 1.0, Attack = 5, Defence = 5 };
                targetUnit = new Robot { HitPoints = 1.0, CurrentHitPoints = 1.0, Attack = 5, Defence = 5 };

                return new CombatCalculator();
            }

            public override void When()
            {
                result = Subject.CalculateDamage(attackUnit, targetUnit);
            }

            [Fact]
            public void The_Damage_Is_Non_Zero()
            {
                Assert.True(result > 0);
            }
        }

        public class When_A_Unit_Attacks_A_Weaker_Unit : SpecificationFor<CombatCalculator>
        {
            private Robot targetUnit;
            private Robot attackUnit;
            private Robot weakerUnit;

            public override CombatCalculator Given()
            {
                attackUnit = new Robot { HitPoints = 1.0, CurrentHitPoints = 1.0, Attack = 5, Defence = 5 };
                targetUnit = new Robot { HitPoints = 1.0, CurrentHitPoints = 1.0, Attack = 5, Defence = 5 };
                weakerUnit = new Robot { HitPoints = 1.0, CurrentHitPoints = 1.0, Attack = 5, Defence = 2 };

                return new CombatCalculator();
            }

            private double targetResult;
            private double weakerResult;

            public override void When()
            {
                targetResult = Subject.CalculateDamage(attackUnit, targetUnit);
                weakerResult = Subject.CalculateDamage(attackUnit, weakerUnit);
            }

            [Fact]
            public void The_Damage_Is_Larger()
            {
                Assert.True(weakerResult > targetResult);
            }
        }

        public class When_A_Unit_Attacks_A_Stronger_Unit : SpecificationFor<CombatCalculator>
        {
            private Robot baselineUnit;
            private Robot attackUnit;
            private Robot strongerUnit;

            public override CombatCalculator Given()
            {
                attackUnit = new Robot { HitPoints = 1.0, CurrentHitPoints = 1.0, Attack = 5, Defence = 5 };
                baselineUnit = new Robot { HitPoints = 1.0, CurrentHitPoints = 1.0, Attack = 5, Defence = 5 };
                strongerUnit = new Robot { HitPoints = 1.0, CurrentHitPoints = 1.0, Attack = 5, Defence = 8 };

                return new CombatCalculator();
            }

            private double baselineResult;
            private double strongerResult;

            public override void When()
            {
                baselineResult = Subject.CalculateDamage(attackUnit, baselineUnit);
                strongerResult = Subject.CalculateDamage(attackUnit, strongerUnit);
            }

            [Fact]
            public void The_Damage_Is_Smaller()
            {
                Assert.True(strongerResult < baselineResult);
            }
        }

    }
}
