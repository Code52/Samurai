using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Impl
{
    public class InMemoryGameStateRepository : IGameStateRepository
    {
        private static Dictionary<Guid, GameState> _state = new Dictionary<Guid, GameState>();

        public GameState Load(Guid id) {
            if (!_state.ContainsKey(id))
                return null;
            return _state[id];
        }

        public void Save(GameState state) {
            state.Id = Guid.NewGuid();
            _state[state.Id] = state;
        }

        public IEnumerable<GameState> ListCurrentGames() {
            return _state.Select(d => d.Value);
        }
    }
}
