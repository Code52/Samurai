using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NSubstitute;
using SamuraiServer.Areas.Api.Controllers;
using SamuraiServer.Data;
using Xunit;

namespace SamuraiServer.Tests.API
{
    public class MatchControllerTests
    {
        public class When_User_Sends_No_Command_For_Match : SpecificationFor<MatchController>
        {
            Guid gameId = Guid.NewGuid();

            public override MatchController Given()
            {
                var repo = Substitute.For<IGameStateRepository>();
                repo.Get(gameId).Returns(new GameState());
                return new MatchController(repo);
            }

            private string userName;
            private IEnumerable<dynamic> commands;

            public override void When()
            {
                var result = Subject.SendCommands(gameId, userName, null) as JsonResult;
                model = result.Data.AsDynamic();
            }

            private dynamic model;

            [Fact]
            public void Result_Is_Ok()
            {
                Assert.True(model.status);
            }
        }

    }
}
