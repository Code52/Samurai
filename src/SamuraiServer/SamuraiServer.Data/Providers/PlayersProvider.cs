using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class PlayersProvider
    {
        private readonly IPlayerRepository _repo;

        public PlayersProvider(IPlayerRepository repository)
        {
            _repo = repository;
        }

        public IEnumerable<Player> GetLeaderboard(int page, int players)
        {
            return _repo.GetAll().Where(p => p.IsActive).OrderByDescending(p => p.Wins).Skip(page*players).Take(players);
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

            _repo.Add(player);

            return p;
        }

        public Player Get(Guid id)
        {
            return _repo.Get(id);
        }

        public void Invite(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Ban(Guid id)
        {
            var player = _repo.GetAll().FirstOrDefault(p => p.IsActive && p.Id == id);
            if(player != null) player.IsActive = false;
            _repo.Save();
        }

        public IEnumerable<Player> Search(string searchTerm)
        {
            var players = _repo.FindBy(p => p.IsActive && p.Name.Contains(searchTerm));

            return players;
        }
    }
}