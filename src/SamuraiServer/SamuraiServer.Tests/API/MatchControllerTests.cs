using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using NSubstitute;
using Newtonsoft.Json;
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
            Guid gameId = Guid.NewGuid();

            public override MatchController Given()
            {
                var repo = Substitute.For<IGameStateRepository>();
                var calculator = Substitute.For<ICombatCalculator>();
                
                repo.Get(gameId).Returns(new GameState());
                return new MatchController(repo, calculator);
            }

            public override void When()
            {
                var result = Subject.SendCommands(gameId, null, null) as JsonResult;
                model = JsonConvert.DeserializeObject<dynamic>(result.SerializeModel());
            }
         
            private dynamic model;

            [Fact]
            public void Result_Is_Ok()
            {
                Assert.Equal(true, model.status.Value);
            }

            [Fact]
            public void Model_Contains_Empty_Units_Array()
            {
                Assert.True(model.data.units.Count == 0);
            }

            [Fact]
            public void Model_Contains_Empty_Errors_Array()
            {
                Assert.True(model.data.errors.Count == 0);
            }

            [Fact]
            public void Model_Contains_Empty_Notifications_Array()
            {
                Assert.True(model.data.notifications.Count == 0);
            }
        }
    }
}
