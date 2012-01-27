using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Threading;
using System.Web.Mvc;
using NSubstitute;
using SamuraiServer.Areas.Api.Controllers;
using SamuraiServer.Data;
using Xunit;

namespace SamuraiServer.Tests.API
{
    public class GamesControllerTests
    {
        private readonly IGameStateProvider _gameStateProvider;
        private readonly GamesController _controller;

        private GameState _dummyGameState;
        private Player _dummyPlayer;

        public GamesControllerTests()
        {
            _dummyGameState = GetDummyGameState();
            _dummyPlayer = GetDummyPlayer();

            _gameStateProvider = Substitute.For<IGameStateProvider>();
            _gameStateProvider.CreateGame(Arg.Any<string>()).ReturnsForAnyArgs(ValidationResult<GameState>.Success.WithData(_dummyGameState));
            _gameStateProvider.JoinGame(Arg.Any<Guid>(), Arg.Any<Guid>()).ReturnsForAnyArgs(ValidationResult<GameState>.Success.WithData(_dummyGameState));

            _controller = new GamesController(_gameStateProvider);
        }

        [Fact]
        public void CreateGameAndJoin_AnyNameAnyUser_CallsProvider()
        {
            //arrange
            var gameName = "someName";

            //act
            var viewResult = _controller.CreateGameAndJoin(gameName, _dummyPlayer.Id) as JsonResult;

            // assert
            Assert.NotNull(viewResult.Data);
            var model = viewResult.Data.AsDynamic();

            Assert.NotNull(model.game);
            Assert.Equal(_dummyGameState, model.game);
            _gameStateProvider.Received().CreateGame(gameName);
            _gameStateProvider.Received().JoinGame(_dummyGameState.Id, _dummyPlayer.Id);
        }

        [Fact]
        public void CreateGameAndJoin_WhenExceptionOccurs_ReturnsFalse()
        {
            //arrange
            var gameName = "someName";
            SetupGameStateProviderException();

            //act
            var viewResult = _controller.CreateGameAndJoin(gameName, _dummyPlayer.Id) as JsonResult;

            //assert
            TestForViewOkFalse(viewResult);

        }

        [Fact]
        public void JoinGame_AnyUser_CallsProvider()
        {
            //arrange
            var gameId = _dummyGameState.Id;

            //act
            var viewResult = _controller.JoinGame(gameId, _dummyPlayer.Id) as JsonResult;

            //assert
            Assert.NotNull(viewResult.Data);
            var model = viewResult.Data.AsDynamic();

            Assert.NotNull(model.game);
            _gameStateProvider.Received().JoinGame(_dummyGameState.Id, _dummyPlayer.Id);

        }

        [Fact]
        public void JoinGame_WhenExceptionOccurs_ReturnsFalse()
        {
            //arrange
            var gameId = _dummyGameState.Id;
            SetupPlayerProviderException();

            //act
            var viewResult = _controller.JoinGame(gameId, _dummyPlayer.Id) as JsonResult;

            //assert
            TestForViewOkFalse(viewResult);
        }

        [Fact]
        public void GetGames_WithUnknownUser_ReturnsErrorCode()
        {
            // act
            var game = _controller.GetGames("") as JsonResult;

            // assert
            dynamic model = game.Data.AsDynamic();
            Assert.False(model.ok);
        }

        [Fact]
        public void ListGames_WhenRepositoryErrorOccurs_ReturnsErrorCode()
        {
            // arrange
            SetupGameStateProviderException();

            // act
            var game = _controller.GetGames("something") as JsonResult;

            // assert
            dynamic model = game.Data.AsDynamic();
            Assert.False(model.ok);
        }

        [Fact]
        public void LeaveGame_ForUserNotInGame_ReturnsErrorCode()
        {
            // arrange
            var gameId = Guid.NewGuid();
            _gameStateProvider.ListCurrentGames(Arg.Any<string>()).Returns(new List<GameState> { _dummyGameState });

            _gameStateProvider.LeaveGame(Arg.Any<Guid>(), Arg.Any<string>()).Returns(ValidationResult<GameState>.Failure("Game does not exist"));
            // act
            var game = _controller.LeaveGame(gameId, "someUser") as JsonResult;

            // assert
            var model = game.Data.AsDynamic();
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
            _gameStateProvider.ListCurrentGames(userName)
              .Returns(results.AsQueryable());

            _gameStateProvider.LeaveGame(Arg.Any<Guid>(), Arg.Any<string>()).Returns(ValidationResult<GameState>.Failure("Player is not in this game"));

            // act
            var game = _controller.LeaveGame(gameId, userName) as JsonResult;

            // assert
            var model = game.Data.AsDynamic();

            Assert.False(model.ok);
            Assert.Equal(model.message, "Player is not in this game");
        }

        [Fact]
        public void LeaveGame_ForUserInCorrectGame_UsesProvider()
        {
            // arrange
            var currentGame = _dummyGameState;

            var results = new[] { currentGame }.AsQueryable();
            _gameStateProvider.ListCurrentGames(Arg.Any<string>())
               .Returns(results);
            
            _gameStateProvider.LeaveGame(Arg.Any<Guid>(), Arg.Any<string>()).Returns(ValidationResult.Success);

            // act
            _controller.LeaveGame(_dummyGameState.Id, _dummyPlayer.Name);

            // assert
            _gameStateProvider.Received().LeaveGame(_dummyGameState.Id, _dummyPlayer.Name);
        }

        // ReSharper restore PossibleNullReferenceException

        private static GameState GetDummyGameState()
        {
            return new GameState
            {
                Id = Guid.NewGuid(),
            };
        }

        private static Player GetDummyPlayer()
        {
            return new Player
            {
                ApiKey = "apikey",
                GamesPlayed = 1,
                Id = Guid.NewGuid(),
                IsActive = true,
                IsOnline = true,
                LastSeen = DateTime.Today.AddDays(42),
                Name = "Jibbr",
                Wins = int.MaxValue
            };
        }

        private void SetupGameStateProviderException()
        {
            _gameStateProvider.When(g => g.CreateGame(Arg.Any<string>())).Do(c =>
            {
                throw new MissingSatelliteAssemblyException();
            });

            _gameStateProvider.When(g => g.ListCurrentGames(Arg.Any<string>())).Do(a =>
            {
                throw new NotFiniteNumberException();
            });
        }

        private void SetupPlayerProviderException()
        {
            _gameStateProvider.When(p => p.JoinGame(Arg.Any<Guid>(), Arg.Any<Guid>())).Do(c =>
            {
                throw new BarrierPostPhaseException();
            });
        }

        private static void TestForViewOkFalse(JsonResult viewResult)
        {
            Assert.NotNull(viewResult.Data);

            var model = viewResult.Data.AsDynamic();
            Assert.False(model.ok);
        }
    }
}
