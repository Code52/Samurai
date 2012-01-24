using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Impl
{
    public class SqlServerPlayerRepository : IPlayerRepository
    {
        public IEnumerable<Player> GetLeaderboard(int page, int players)
        {
            throw new NotImplementedException();
        }

        public Player Create(Player player)
        {
            throw new NotImplementedException();
        }

        public Player Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Invite(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Ban(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Player> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
