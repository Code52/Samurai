using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Impl
{
    public class SqlServerGameStateRepository : IGameStateRepository
    {
        // TODO: We may also store our json in SQL nvarchar(MAX) if that is easier for people

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
