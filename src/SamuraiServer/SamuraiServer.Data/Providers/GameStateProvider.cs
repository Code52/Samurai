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

        public ValidationResult Save(GameState state)
        {
            var gs = _repo.FindBy(s => s.Id == state.Id);

            if (gs == null)
            {
                return ValidationResult.Failure("Game not found");
            }

            _repo.Add(state);
            _repo.Save();

            return ValidationResult.Success;
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

        public ValidationResult<GameState> CreateGame(string name)
        {
            if (_repo.GetByName(name) != null)
                return ValidationResult<GameState>.Failure("A game by that name already exists");

            var id = Guid.NewGuid();
            
            var gameState = new GameState { Id = id, Name = name, MapId = _mapProvider.GetRandomMap().Id, Started = false };
            
            _repo.Add(gameState);
            _repo.Save();

            return ValidationResult<GameState>.Success.WithData(gameState);
        }

        public ValidationResult<GameState> JoinGame(Guid gameId, Guid playerId)
        {
            if (gameId == Guid.Empty)
            {
                return ValidationResult<GameState>.Failure("gameId not set");
            }

            if (playerId == Guid.Empty)
            {
                return ValidationResult<GameState>.Failure("playerId not set");
            }

            var player = _playersProvider.Get(playerId);
            var game = _repo.Get(gameId);

            if (player == null) return ValidationResult<GameState>.Failure("Could not find Player");
            if (game == null) return ValidationResult<GameState>.Failure("Could not find Game");

            if (game.Players.Any(p => p.Player.Id == player.Id))
                return ValidationResult<GameState>.Failure("Player is already a member of that game");

            if (game.Started) return ValidationResult<GameState>.Failure("Cannot join a game in progress");

            game.Players.Add(new GamePlayer { Id = Guid.NewGuid(), Player = player, IsAlive = true, Score = 0 });
            _repo.Save();

            return ValidationResult<GameState>.Success.WithData(game);
        }

        public ValidationResult LeaveGame(Guid gameId, string userName)
        {
            var currentGame = this.ListCurrentGames(userName).FirstOrDefault(g => g.Id == gameId);

            if (currentGame == null)
                return ValidationResult.Failure("Game does not exist");

            var player = currentGame.Players.FirstOrDefault(f => f.Player.Name == userName);

            if (player == null)
                return ValidationResult.Failure("Player is not in this game");

            currentGame.Players.Remove(player);

            this.Save(currentGame);

            return ValidationResult.Success;
        }

        public ValidationResult<string[]> GetMap(Guid mapId)
        {
            try
            {
                var map = _mapProvider.Get(mapId);
                return ValidationResult<string[]>.Success.WithData(map.GetStringRepresentation());
            }
            catch
            {
                return ValidationResult<string[]>.Failure("Invalid Map Id");
            }
        }

        public ValidationResult<GameState> StartGame(Guid gameId)
        {
            if (gameId == Guid.Empty) return ValidationResult<GameState>.Failure("gameId needs to be set");

            var game = _repo.Get(gameId);

            if (game == null) return ValidationResult<GameState>.Failure("Game could not be found");

            if(game.Players.Count < 2)
                return ValidationResult<GameState>.Failure("Game must have at least 2 players to start");

            if (game.Started == false)  // Multiple users may try to start a game, return success but don't do anything to the game state
            {
                    
                // Sort players
                var random = new Random();
                game.PlayerOrder = game.Players.OrderBy(d => random.Next(0, 100)).Select(d => d.Id).ToList();

                game.Turn = 0;
                game.Started = true;

                _repo.Save();
            }

            return ValidationResult<GameState>.Success.WithData(game);
        }
    }
}
