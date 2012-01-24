using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Impl
{
    public class InMemoryGameStateRepository : IGameStateRepository
    {
        private Dictionary<Guid, GameState> _state = new Dictionary<Guid, GameState>();

        public GameState Load(Guid id) {
            return _state[id];
        }

        public void Save(GameState state) {
            _state[state.Id] = state;
        }
    }
}
