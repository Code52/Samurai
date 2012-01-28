using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class CombatCalculatorTests
    {
        public class When_A_Unit_Attacks_A_Similar_Unit : SpecificationFor<CombatCalculator>
        {
            public override CombatCalculator Given()
            {
                return new CombatCalculator();
            }

            public override void When()
            {
                
            }

            [Fact]
            public void Something()
            {
                Assert.True(false);
            }
        }

    }
}
