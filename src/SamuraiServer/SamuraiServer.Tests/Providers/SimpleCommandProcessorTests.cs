using System;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    internal class Ninja : Unit { }

    public class SimpleCommandProcessorTests
    {
        public class When_A_Unit_Moves_More_Than_Its_Allowed_Range : TwoPlayerGame
        {
            Guid id = Guid.NewGuid();
            Ninja activeUnit;
            Unit modifiedUnit;

            public override CommandProcessor Given()
            {
                activeUnit = new Ninja { Id = id, X = 0, Y = 0 };
                activeUnit.Range = 1;

                FirstPlayer.Units.Add(activeUnit);

                return new CommandProcessor(State);
            }

            public override void When()
            {
                modifiedUnit = Subject.MoveUnit(id, 1, 1);
            }

            [Fact]
            public void The_Result_Is_Null()
            {
                Assert.Null(modifiedUnit);
            }
        }
    }
}