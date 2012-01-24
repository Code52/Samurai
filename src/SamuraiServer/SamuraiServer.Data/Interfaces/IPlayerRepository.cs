using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetLeaderboard(int page, int players);

        Player Create(Player player);

        Player Get(Guid id);

        void Invite(Guid id);

        void Ban(Guid id);

        IEnumerable<Player> Search(string searchTerm);
    }
}
