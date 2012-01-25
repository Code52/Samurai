using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using NSubstitute;
using SamuraiServer.Areas.Api.Controllers;
using SamuraiServer.Data;
using Xunit;

namespace SamuraiServer.Tests.API
{
    public class GamesControllerTests
    {
        // ReSharper disable PossibleNullReferenceException
        private readonly GamesController _controller;
        private readonly IGameStateRepository _db;
        private readonly GameStateProvider _prov;

        public GamesControllerTests()
        {
            _db = Substitute.For<IGameStateRepository>();
            _prov = new GameStateProvider(_db);
            _controller = new GamesController(_prov);
        }

        [Fact]
        public void CreateGame_ForSomeUser_AddsRecordToDatabase()
        {
            // act
            var game = _controller.CreateGame("someUser") as ViewResult;

            // assert
            Assert.NotNull(game.Model);
            _db.Received().Add(Arg.Any<GameState>());
        }

        [Fact]
        public void GetGames_WithUnknownUser_DoesNotQueryRepository()
        {
            // act
            _controller.GetGames("");

            // assert
            _db.DidNotReceiveWithAnyArgs().FindBy(null);
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
            _db.Received().GetAll();
        }

        [Fact]
        public void ListGames_WhenRepositoryErrorOccurs_ReturnsErrorCode()
        {
            // arrange
            _db.When(db => db.FindBy(Arg.Any<Expression<Func<GameState, bool>>>()))
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
            _db.FindBy(Arg.Any<Expression<Func<GameState, bool>>>())
               .Returns(new GameState[0].AsQueryable());

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

            const string userName = "someUser";
            var results = new[] { new GameState { Id = gameId } };
            _db.FindBy(Arg.Any<Expression<Func<GameState, bool>>>())
              .Returns(results.AsQueryable());

            // act
            var game = _controller.LeaveGame(gameId, userName) as ViewResult;

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
            var results = new[] { currentGame }.AsQueryable();
            _db.FindBy(Arg.Any<Expression<Func<GameState, bool>>>())
               .Returns(results);

            // act
            _controller.LeaveGame(gameId, userName);

            // assert
            _db.Received().Add(currentGame);
        }
        // ReSharper restore PossibleNullReferenceException
    }
}
