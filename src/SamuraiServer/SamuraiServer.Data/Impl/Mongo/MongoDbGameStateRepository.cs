using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Impl
{
    public class MongoDbGameStateRepository : IGameStateRepository
    {
        // TODO: If we decide to launch with MongoDb then we need to be able to store our game state

        public GameState Load(Guid id) {
            throw new NotImplementedException();
        }

        public void Save(GameState state) {
            throw new NotImplementedException();
        }

        public IEnumerable<GameState> ListCurrentGames() {
            throw new NotImplementedException();
        }
    }
}
