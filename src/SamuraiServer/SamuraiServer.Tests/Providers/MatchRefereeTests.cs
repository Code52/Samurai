using System;
using System.Collections.Generic;
using IdeaStrike.Tests;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class Ninja : Unit { }

    public class SimpleCommandProcessorTests
    {
        public class When_Moving_A_Unit_In_A_Game : TwoPlayerGame
        {
            readonly Guid id = Guid.NewGuid();
            Ninja activeUnit;
            Unit adjustedUnit;

            public override CommandProcessor Given()
            {
                activeUnit = new Ninja { Id = id, X = 0, Y = 0, Range = 1 };

                FirstPlayer.Units.Add(activeUnit);

                return new CommandProcessor(State);
            }

            public override void When()
            {
                adjustedUnit = Subject.MoveUnit(id, 1, 0);
            }

            [Fact]
            public void The_Unit_Received_Is_The_Original_Unit()
            {
                Assert.Equal(adjustedUnit.Id, activeUnit.Id);
            }

            [Fact]
            public void The_Unit_Has_Its_Position_Set()
            {
                Assert.Equal(1, adjustedUnit.X);
                Assert.Equal(0, adjustedUnit.Y);
            }
        }

        public class When_A_Player_Moves_Out_Of_Order : TwoPlayerGame
        {
            readonly Guid id = Guid.NewGuid();
            Ninja activeUnit;
            Ninja otherUnit;
            Unit adjustedUnit;

            public override CommandProcessor Given()
            {
                activeUnit = new Ninja { Id = id, X = 0, Y = 0 };
                otherUnit = new Ninja { Id = Guid.NewGuid(), X = 1, Y = 1 };

                FirstPlayer.Units.Add(activeUnit);
                SecondPlayer.Units.Add(otherUnit);

                State.Turn = State.Players.IndexOf(SecondPlayer);

                return new CommandProcessor(State);
            }

            public override void When()
            {
                adjustedUnit = Subject.MoveUnit(id, 1, 0);
            }

            [Fact]
            public void The_Unit_Received_Is_Null()
            {
                Assert.Null(adjustedUnit);
            }
        }

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

    public class ComplexCommandProcessorTests
    {

        public class When_Sending_A_Move_As_JSON : SpecificationFor<CommandProcessor>
        {
            public override CommandProcessor Given()
            {
                throw new NotImplementedException();
            }

            public override void When()
            {
                throw new NotImplementedException();
            }
        }
    }
}