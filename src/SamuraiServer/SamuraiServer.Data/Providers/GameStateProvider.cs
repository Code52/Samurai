using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class GameStateProvider : IGameStateProvider
    {
        private readonly IGameStateRepository _repo;
        private readonly IPlayersProvider _playersProvider;
        private readonly IMapProvider _mapProvider;

        public GameStateProvider(IGameStateRepository repository, IPlayersProvider playersProvider, IMapProvider mapProvider)
        {
            _repo = repository;
            _playersProvider = playersProvider;
            _mapProvider = mapProvider;
        }

        public GameState Load(Guid id)
        {
            return _repo.Get(id);
        }

        public void Save(GameState state)
        {
            var gs = _repo.FindBy(s => s.Id == state.Id);

            if (gs == null)
            {
                throw new ArgumentException("Game not found");
            }

            _repo.Add(state);
        }

        public IEnumerable<GameState> ListOpenGames()
        {
            return _repo.GetAll();
        }

        public IEnumerable<GameState> ListCurrentGames(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
            {
                return Enumerable.Empty<GameState>();
            }
            return _repo.FindBy(d => d.Players.Any(c => c.Player.Name == userName));
        }

        public GameState CreateGame(string name)
        {
            var id = Guid.NewGuid();
            var gameState = new GameState { Id = id, Name = name, Map = _mapProvider.GetRandomMap() };
            _repo.Add(gameState);
            return gameState;
        }

        public GameState JoinGame(Guid gameId, Guid playerId)
        {
            if (gameId == Guid.Empty)
            {
                throw new ArgumentException("gameId not set");
            }

            if (playerId == Guid.Empty)
            {
                throw new ArgumentException("gameId not set");
            }

            var player = _playersProvider.Get(playerId);
            var game = _repo.Get(gameId);
            game.Players.Add(new GamePlayer { Id = Guid.NewGuid(), Player = player, IsAlive = true, Score = 0 });
            _repo.Save();
            return game;
        }
    }
}
