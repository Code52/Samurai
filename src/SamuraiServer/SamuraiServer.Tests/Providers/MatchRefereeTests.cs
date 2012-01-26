using System;
using System.Collections.Generic;
using IdeaStrike.Tests;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class UmpireTests
    {
        public class Ninja : Unit { }

        public class When_Moving_A_Unit_In_A_Game : SpecificationFor<MatchReferee>
        {
            readonly Guid id = Guid.NewGuid();
            
            GameState match;
            Ninja activeUnit;
            Unit adjustedUnit;

            public override MatchReferee Given()
            {
                activeUnit = new Ninja { Id = id, X = 0, Y = 0 };
                activeUnit.Range = 1;

                var firstPlayer = new GamePlayer();
                firstPlayer.Units.Add(activeUnit);

                match = new GameState();
                match.Players.Add(firstPlayer);
                match.Players.Add(new GamePlayer());

                return new MatchReferee(match);
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

        public class When_A_Player_Moves_Out_Of_Order : SpecificationFor<MatchReferee>
        {
            readonly Guid id = Guid.NewGuid();
            Ninja activeUnit;
            Ninja otherUnit;
            Unit adjustedUnit;

            public override MatchReferee Given()
            {
                activeUnit = new Ninja { Id = id, X = 0, Y = 0 };
                otherUnit = new Ninja { Id = Guid.NewGuid(), X = 1, Y = 1 };
                var firstPlayer = new GamePlayer();
                firstPlayer.Units.Add(activeUnit);
                var secondPlayer = new GamePlayer();
                secondPlayer.Units.Add(otherUnit);

                var match = new GameState();
                match.Players = new List<GamePlayer> { firstPlayer, secondPlayer };
                match.Turn = match.Players.IndexOf(secondPlayer);

                return new MatchReferee(match);
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


        public class When_A_Unit_Moves_More_Than_Its_Allowed_Range : SpecificationFor<MatchReferee>
        {
            Guid id = Guid.NewGuid();
            Ninja activeUnit;
            GameState match;
            Unit modifiedUnit;

            public override MatchReferee Given()
            {
                activeUnit = new Ninja { Id = id, X = 0, Y = 0 };
                activeUnit.Range = 1;
                var firstPlayer = new GamePlayer();
                firstPlayer.Units.Add(activeUnit);

                match = new GameState();
                match.Players.Add(firstPlayer);
                match.Players.Add(new GamePlayer());

                return new MatchReferee(match);               
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