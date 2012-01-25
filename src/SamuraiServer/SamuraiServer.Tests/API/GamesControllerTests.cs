using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly GameStateProvider _prov;

        public GamesControllerTests()
        {
            _db = Substitute.For<IGameStateRepository>();
            _prov = Substitute.For<GameStateProvider>(_db);
            _controller = new GamesController(_prov);
        }

        [Fact]
        public void CreateGame_ForSomeUser_AddsRecordToDatabase()
        {
            // act
            var game = _controller.CreateGame("someUser") as ViewResult;

            // assert
            Assert.NotNull(game.Model);
            _prov.Received().Save(Arg.Any<GameState>());
        }

        [Fact]
        public void GetGames_WithUnknownUser_DoesNotQueryRepository()
        {
            // act
            _controller.GetGames("");

            // assert
            _prov.DidNotReceive().ListCurrentGames(Arg.Any<string>());
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
            _prov.Received().ListOpenGames();
        }

        [Fact]
        public void ListGames_WhenRepositoryErrorOccurs_ReturnsErrorCode()
        {
            // arrange
            _prov.When(db => db.ListCurrentGames(Arg.Any<string>()))
               .Do(c => { throw new Exception("oops"); });

            // act
            var game = _controller.GetGames("something") as ViewResult;

            // assert
            dynamic model = game.Model.AsDynamic();
            Assert.False(model.ok);
        }

        [Fact]
        public void LeaveGame_ForUserNotInGame_ReturnsErrorCode()
        {
            // arrange
            var gameId = Guid.NewGuid();
            _prov.ListCurrentGames("someUser").Returns(new GameState[0]);

            // act
            var game = _controller.LeaveGame(gameId, "someUser") as ViewResult;

            // assert
            var model = game.Model.AsDynamic();
            Assert.False(model.ok);
            Assert.Equal(model.message, "Game does not exist");
        }

        [Fact]
        public void LeaveGame_ForUserNotInGame_ReturnsFalse()
        {
            // arrange
            var gameId = Guid.NewGuid();
            
            _prov.ListCurrentGames("someUser").Returns(new[] { new GameState { Id = gameId } });

            // act
            var game = _controller.LeaveGame(gameId, "someUser") as ViewResult;

            // assert
            var model = game.Model.AsDynamic();
            Assert.False(model.ok);
            Assert.Equal(model.message, "Player is not in this game");
        }

        [Fact]
        public void LeaveGame_ForUserInCorrectGame_UsesRepository()
        {
            // arrange
            var gameId = Guid.NewGuid();
            const string userName = "someUser";
            var currentGame = new GameState
                                  {
                                      Id = gameId,
                                      Players = new List<GamePlayer>
                                                    {
                                                        new GamePlayer { Player = new Player { Name = userName } }
                                                    }
                                  };
            _prov.ListCurrentGames(userName).Returns(new[] { currentGame });

            // act
            var game = _controller.LeaveGame(gameId, userName) as ViewResult;

            // assert
            var model = game.Model.AsDynamic();
            _prov.Received().Save(new[] { currentGame }.First());
        }
    }
}
