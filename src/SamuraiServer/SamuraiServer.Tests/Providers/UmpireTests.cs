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
            readonly Guid _id = Guid.NewGuid();
            private Ninja _activeUnit;
            

            public override MatchReferee Given()
            {
                _activeUnit = new Ninja { Id = _id, X = 0, Y = 0 };

                var firstPlayer = new GamePlayer();
                firstPlayer.Units.Add(_activeUnit);
                var secondPlayer = new GamePlayer();

                var match = new GameState();
                match.Players = new List<GamePlayer> { firstPlayer, secondPlayer };

                return new MatchReferee(match);
            }

            private Unit _adjustedUnit;

            public override void When()
            {
                _adjustedUnit = Subject.MoveUnit(_id, 1, 0);
            }

            [Fact]
            public void The_Unit_Received_Is_The_Original_Unit()
            {
                Assert.Equal(_adjustedUnit.Id, _activeUnit.Id);
            }

            [Fact]
            public void The_Unit_Has_Its_Position_Set()
            {
                Assert.Equal(1, _adjustedUnit.X);
                Assert.Equal(0, _adjustedUnit.Y);
            }
        }
    }
}