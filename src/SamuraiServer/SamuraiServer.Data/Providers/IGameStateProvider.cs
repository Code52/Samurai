using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public interface IGameStateProvider
    {
        GameState Load(Guid id);
        void Save(GameState state);
        IEnumerable<GameState> ListOpenGames();
        IEnumerable<GameState> ListCurrentGames(string userName);
        GameState CreateGame(string name);
        GameState JoinGame(Guid gameId, Guid playerId);
    }
}
