using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public interface IGameStateRepository
    {
        GameState Load(Guid id);
        void Save(GameState state);
    }
}
