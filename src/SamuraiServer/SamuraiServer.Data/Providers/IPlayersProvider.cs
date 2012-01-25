using System;
using System.Collections.Generic;

namespace SamuraiServer.Data
{
    public interface IPlayersProvider
    {
        IEnumerable<Player> GetLeaderboard(int page, int players);
        Player Create(string name);
        Player Get(Guid id);
        void Invite(Guid id);
        void Ban(Guid id);
        IEnumerable<Player> Search(string searchTerm);
    }
}