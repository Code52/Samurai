using System;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class CombatCalculatorTests
    {
        public class Robot : Unit {
            
            public Robot()
            {
                Id = Guid.NewGuid();
                Attack = 5;
                Defence = 5;
                Range = 1;
                HitPoints = 1;
                CurrentHitPoints = 1;
            }

            public override string Name
            {
                get { return "Robot"; }
            }

            public override string ImageSpriteResource
            {
                get { throw new NotImplementedException(); }
            }
        }

        public class When_A_Unit_Attacks_A_Similar_Unit : SpecificationFor<CombatCalculator>
        {
            private Robot targetUnit;
            private Robot attackUnit;
            private double result;

            public override CombatCalculator Given()
            {
                attackUnit = new Robot() { };
                targetUnit = new Robot { };

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
                attackUnit = new Robot { };
                targetUnit = new Robot { };
                weakerUnit = new Robot { Defence = 2};

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
                attackUnit = new Robot {  };
                baselineUnit = new Robot {  };
                strongerUnit = new Robot { Defence = 8 };

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
