using System;
using NSubstitute;
using SamuraiServer.Data;
using Xunit;

namespace SamuraiServer.Tests.Providers
{
    public class GameStateProviderTests
    {
        // ReSharper disable PossibleNullReferenceException
        private readonly IGameStateRepository _gameStateRepository;
        private readonly GameStateProvider _gameStateProvider;
        private readonly IPlayersProvider _playersProvider;
        private readonly IMapProvider _mapProvider;
        private readonly Player _dummyPlayer;
        private readonly Map _dummyMap;

        public GameStateProviderTests()
        {
            _dummyPlayer = GetDummyPlayer();
            _dummyMap = GetDummyMap();

            _gameStateRepository = Substitute.For<IGameStateRepository>();
            _playersProvider = Substitute.For<IPlayersProvider>();
            _playersProvider.Get(Guid.Empty).ReturnsForAnyArgs(_dummyPlayer);
            _mapProvider = Substitute.For<IMapProvider>();
            _mapProvider.GetRandomMap().ReturnsForAnyArgs(_dummyMap);
            _gameStateProvider = new GameStateProvider(_gameStateRepository, _playersProvider, _mapProvider);
        }

        [Fact]
        public void CreateGame_ForSomeUser_AddsRecordToDatabase()
        {
            // act
            var game = _gameStateProvider.CreateGame("someUser");

            // assert
            Assert.NotNull(game);
            _gameStateRepository.Received().Add(Arg.Any<GameState>());
        }

        [Fact]
        public void CreateGame_ForSomeUser_GetsCreatedWithMap()
        {
            var game = _gameStateProvider.CreateGame("foobar");

            Assert.NotNull(game);
            Assert.True(game.IsValid.Value);
            Assert.NotNull(game.Data.Map);
            _mapProvider.Received().GetRandomMap();
        }

        [Fact]
        public void GetGames_WithUnknownUser_DoesNotQueryRepository()
        {
            // act
            _gameStateProvider.ListCurrentGames("");

            // assert
            _gameStateRepository.DidNotReceiveWithAnyArgs().FindBy(null);
        }

        [Fact]
        public void ListGames_ForAnyUser_QueriesRepository()
        {
            // act
            var game = _gameStateProvider.ListOpenGames();

            // assert
            Assert.NotNull(game);
            _gameStateRepository.Received().GetAll();
        }

        // ReSharper restore PossibleNullReferenceException

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
    }
}
