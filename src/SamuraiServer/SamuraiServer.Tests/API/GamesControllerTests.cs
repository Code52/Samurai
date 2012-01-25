using System;
using System.Web.Mvc;
using NSubstitute;
using SamuraiServer.Areas.Api.Controllers;
using SamuraiServer.Data;
using Xunit;

namespace SamuraiServer.Tests.API
{
    public class GamesControllerTests
    {
        private readonly GamesController _controller;
        private readonly IGameStateRepository _db;

        public GamesControllerTests()
        {
            _db = Substitute.For<IGameStateRepository>();
            _controller = new GamesController(_db);
        }

        [Fact]
        public void CreateGame_ForSomeUser_AddsRecordToDatabase()
        {
            // act
            var game = _controller.CreateGame("someUser") as ViewResult;

            // assert
            Assert.NotNull(game.Model);
            _db.Received().Save(Arg.Any<GameState>());
        }

        [Fact]
        public void GetGames_WithUnknownUser_DoesNotQueryRepository()
        {
            // act
            _controller.GetGames("");

            // assert
            _db.DidNotReceive().ListCurrentGames(Arg.Any<string>());
        }

        [Fact]
        public void GetGames_WithUnknownUser_ReturnsErrorCode()
        {
            // act
            var game = _controller.GetGames("") as ViewResult;

            // assert
            dynamic model = game.Model.AsDynamic();
            Assert.False(model.ok);
        }

        [Fact]
        public void ListGames_ForAnyUser_QueriesRepository()
        {
            // act
            var game = _controller.GetOpenGames() as ViewResult;

            // assert
            Assert.NotNull(game.Model);
            _db.Received().ListOpenGames();
        }

        [Fact]
        public void ListGames_WhenRepositoryErrorOccurs_ReturnsErrorCode()
        {
            _db.When(db => db.ListCurrentGames(Arg.Any<string>()))
               .Do(c => { throw new Exception("oops"); });

            // act
            var game = _controller.GetGames("something") as ViewResult;

            // assert
            dynamic model = game.Model.AsDynamic();
            Assert.False(model.ok);
        }
    }
}
