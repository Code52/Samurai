using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public interface IGameStateProvider
    {
        GameState Load(Guid id);
        ValidationResult Save(GameState state);
        IEnumerable<GameState> ListOpenGames();
        IEnumerable<GameState> ListCurrentGames(string userName);
        ValidationResult<GameState> CreateGame(string name);
        ValidationResult<GameState> JoinGame(Guid gameId, Guid playerId);
        ValidationResult LeaveGame(Guid gameId, string userName);
        ValidationResult<string[]> GetMap(Guid mapId);
    }
}
