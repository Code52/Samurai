using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data.Impl
{
    public class InMemoryPlayerRepository: IPlayerRepository
    {
        private static List<Player> _players = new List<Player>(); 

        public IEnumerable<Player> GetLeaderboard(int page, int players)
        {
            return _players.Where(p => p.IsActive).OrderByDescending(p => p.Wins).Skip(page*players).Take(players);
        }

        public Player Create(Player player)
        {
            var random = new Random();

            var p = player;
            
            p.Id = Guid.NewGuid();
            p.IsOnline = true;
            p.LastSeen = DateTime.Now;
            p.ApiKey = string.Format("{0}{1}{2}{3}{4}",
                                     random.Next(0, 9),
                                     random.Next(0, 9),
                                     random.Next(0, 9),
                                     random.Next(0, 9),
                                     random.Next(0, 9));

            p.IsActive = true;

            _players.Add(p);

            return p;
        }

        public Player Get(Guid id)
        {
            return _players.FirstOrDefault(p => p.IsActive && p.Id == id);
        }

        public void Invite(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Ban(Guid id)
        {
            var player = _players.FirstOrDefault(p => p.IsActive && p.Id == id);
            player.IsActive = false;
        }

        public IEnumerable<Player> Search(string searchTerm)
        {
            var players = _players.Where(p => p.IsActive && p.Name.Contains(searchTerm));

            return players;
        }
    }
}
