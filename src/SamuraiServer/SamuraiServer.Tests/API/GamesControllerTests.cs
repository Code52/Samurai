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
        private readonly IPlayersProvider _playersProvider;
        private readonly GamesController _controller;

        private GameState _dummyGameState;
        private Player _dummyPlayer;
        private Map _dummyMap;

        public GamesControllerTests()
        {
            _dummyGameState = GetDummyGameState();
            _dummyPlayer = GetDummyPlayer();
            _dummyMap = GetDummyMap();

            _gameStateProvider = Substitute.For<IGameStateProvider>();
            _gameStateProvider.CreateGame(Arg.Any<string>()).ReturnsForAnyArgs(_dummyGameState);
            _gameStateProvider.JoinGame(Arg.Any<Guid>(), Arg.Any<Guid>()).ReturnsForAnyArgs(_dummyGameState);

            _playersProvider = Substitute.For<IPlayersProvider>();
            _playersProvider.Create(Arg.Any<string>()).ReturnsForAnyArgs(_dummyPlayer);


            _controller = new GamesController(_gameStateProvider, _playersProvider);
        }

        // ReSharper disable PossibleNullReferenceException
        [Fact]
        public void CreateGame_AnyNameAnyUser_CallsProvider()
        {
            //arrange
            var gameName = "someName";

            //act
            var viewResult = _controller.CreateGame(gameName) as ViewResult;

            // assert
            Assert.NotNull(viewResult.Model);
            var model = viewResult.Model.AsDynamic();

            Assert.NotNull(model.game);
            Assert.Equal(_dummyGameState, model.game);
            _gameStateProvider.Received().CreateGame(gameName);
        }

        [Fact]
        public void CreateGame_WhenExceptionOccurs_ReturnsFalse()
        {
            //arrange
            var gameName = "someName";
            SetupGameStateProviderException();

            //act
            var viewResult = _controller.CreateGame(gameName) as ViewResult;

            //assert
            TestForViewOkFalse(viewResult);
        }

        [Fact]
        public void CreateGameAndJoin_AnyNameAnyUser_CallsProvider()
        {
            //arrange
            var gameName = "someName";

            //act
            var viewResult = _controller.CreateGameAndJoin(gameName, _dummyPlayer.Id) as ViewResult;

            // assert
            Assert.NotNull(viewResult.Model);
            var model = viewResult.Model.AsDynamic();

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
            var viewResult = _controller.CreateGameAndJoin(gameName, _dummyPlayer.Id) as ViewResult;

            //assert
            TestForViewOkFalse(viewResult);

        }

        [Fact]
        public void JoinGame_AnyUser_CallsProvider()
        {
            //arrange
            var gameId = _dummyGameState.Id;

            //act
            var viewResult = _controller.JoinGame(gameId, _dummyPlayer.Name) as ViewResult;

            //assert
            Assert.NotNull(viewResult.Model);
            var model = viewResult.Model.AsDynamic();

            Assert.NotNull(model.game);
            Assert.NotNull(model.player);
            _playersProvider.Received().Create(_dummyPlayer.Name);
            _gameStateProvider.Received().JoinGame(_dummyGameState.Id, _dummyPlayer.Id);

        }

        [Fact]
        public void JoinGame_WhenExceptionOccurs_ReturnsFalse()
        {
            //arrange
            var gameId = _dummyGameState.Id;
            SetupPlayerProviderException();

            //act
            var viewResult = _controller.JoinGame(gameId, _dummyPlayer.Name) as ViewResult;

            //assert
            TestForViewOkFalse(viewResult);
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
        public void ListGames_WhenRepositoryErrorOccurs_ReturnsErrorCode()
        {
            // arrange
            SetupGameStateProviderException();

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
            _gameStateProvider.ListCurrentGames(Arg.Any<string>()).Returns(new GameState[0]);

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
            _gameStateProvider.ListCurrentGames(userName)
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
            _gameStateProvider.ListCurrentGames(Arg.Any<string>())
               .Returns(results);

            // act
            _controller.LeaveGame(gameId, userName);

            // assert
            _gameStateProvider.Received().Save(results.First());
            Assert.Equal(0,results.First().Players.Count);
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

        private static Map GetDummyMap()
        {
            return new Map
            {
                ImageResource = "Swashbucklin.jpg",
                Name = "Booty",
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
            _playersProvider.When(p => p.Create(Arg.Any<string>())).Do(c =>
            {
                throw new BarrierPostPhaseException();
            });
        }

        private static void TestForViewOkFalse(ViewResult viewResult)
        {
            Assert.NotNull(viewResult.Model);

            var model = viewResult.Model.AsDynamic();
            Assert.False(model.ok);
        }
    }
}
