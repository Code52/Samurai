using System;
using NSubstitute;
using SamuraiServer.Data;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class PlayersProviderTests
    {
        public class When_Creating_A_New_Player : SpecificationFor<PlayersProvider>
        {
            IPlayerRepository _repo;

            public override PlayersProvider Given()
            {
                _repo = Substitute.For<IPlayerRepository>();
                return new PlayersProvider(_repo);
            }

            const string PlayerName = "Name";
            ValidationResult<Player> _player;

            public override void When()
            {
                _player = Subject.Create(PlayerName);
            }

            [Fact]
            public void The_Name_Matches_The_Input_Parameter()
            {
                Assert.Equal(PlayerName, _player.Data.Name);
            }

            [Fact]
            public void The_Id_Is_Not_Empty()
            {
                Assert.NotEqual(Guid.Empty, _player.Data.Id);
            }

            [Fact]
            public void The_Repository_Receives_A_Player()
            {
                _repo.Received().Add(Arg.Any<Player>());
            }
        }
    }
}
