using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Impl
{
    public class FileSystemGameStateRepository : IGameStateRepository
    {
        // TODO: Create a testing repo that serializes states to disk. This allows testing that can survive an app restart

        public GameState Load(Guid id) {
            throw new NotImplementedException();
        }

        public void Save(GameState state) {
            throw new NotImplementedException();
        }
    }
}
