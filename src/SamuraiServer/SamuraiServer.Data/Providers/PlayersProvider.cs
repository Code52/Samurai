using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public class PlayersProvider : IPlayersProvider
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

        public ValidationResult<Player> Create(string name)
        {
            if (_repo.GetByName(name) != null) return ValidationResult<Player>.Failure("User already exists");

            var random = new Random();

            var player = new Player{Name = name};
            
            player.Id = Guid.NewGuid();
            player.IsOnline = true;
            player.LastSeen = DateTime.Now;
            player.ApiKey = string.Format("{0}{1}{2}{3}{4}",
                                     random.Next(0, 9),
                                     random.Next(0, 9),
                                     random.Next(0, 9),
                                     random.Next(0, 9),
                                     random.Next(0, 9));

            player.IsActive = true;

            _repo.Add(player);
            _repo.Save();

            return ValidationResult<Player>.Success.WithData(player);
        }

        public ValidationResult<Player> Login(string name, string apiKey)
        {
            Player player = _repo.GetByName(name);

            // See if player 1) wasn't found or 2) ApiKey doesn't match
            if (player == null || string.CompareOrdinal(apiKey, player.ApiKey) != 0)
                return ValidationResult<Player>.Failure("User with specified key wasn't found.");

            player.IsOnline = true;

            return ValidationResult<Player>.Success.WithData(player);
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