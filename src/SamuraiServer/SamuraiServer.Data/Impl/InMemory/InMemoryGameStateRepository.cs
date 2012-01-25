using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiServer.Data.Impl
{
    public class InMemoryGameStateRepository : IGameStateRepository
    {
        private static Dictionary<Guid, GameState> _state = new Dictionary<Guid, GameState>();

        public GameState Load(Guid id)
        {
            if (!_state.ContainsKey(id))
                return null;
            return _state[id];
        }

        public void Save(GameState state)
        {
            state.Id = Guid.NewGuid();
            _state[state.Id] = state;
        }

        public IEnumerable<GameState> ListOpenGames()
        {
            return _state.Select(d => d.Value);
        }

        public IEnumerable<GameState> ListCurrentGames(string userName)
        {
            return _state.Where(d => d.Value.Players.Any(c => c.Player.Name == userName))
                         .Select(d => d.Value);
        }
    }
}
