using System;
using System.Web.Mvc;
using NSubstitute;
using SamuraiServer.Areas.Api.Controllers;
using SamuraiServer.Data;
using SamuraiServer.Data.Providers;
using Xunit;

namespace SamuraiServer.Tests.API
{
    public class MatchControllerTests
    {
        public class When_User_Sends_No_Command_For_Match : SpecificationFor<MatchController>
        {
            readonly GameState state = new GameState();
            readonly Guid gameId = Guid.NewGuid();

            IGameStateRepository repo = Substitute.For<IGameStateRepository>();
            dynamic model;

            public override MatchController Given()
            {
                var calculator = Substitute.For<ICombatCalculator>();
                repo.Get(gameId).Returns(state);
                return new MatchController(repo, calculator);
            }

            public override void When()
            {
                var result = Subject.SendCommands(gameId, null, null) as JsonResult;
                model = result.Data.AsDynamic();
            }
            
            [Fact]
            public void Result_Is_Ok()
            {
                Assert.True(model.status);
            }

            [Fact]
            public void Repository_Saves_Game()
            {
                repo.Received().Edit(state);
                repo.Received().Save();
            }
        }
    }
}
