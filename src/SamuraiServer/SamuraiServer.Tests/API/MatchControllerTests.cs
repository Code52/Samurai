﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using IdeaStrike.Tests;
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
            public override MatchController Given()
            {
                IGameStateRepository repo = Substitute.For<IGameStateRepository>();
                return new MatchController(repo);
            }

            private Guid gameId;
            private string userName;
            private IEnumerable<dynamic> commands;

            public override void When()
            {
                var result = Subject.SendCommand(gameId, userName, commands) as ViewResult;
                model = result.Model.AsDynamic();
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