using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiServer.Data
{
    public class PlayersProvider : IPlayersProvider
    {
        private readonly IPlayerRepository repo;

        public PlayersProvider(IPlayerRepository repository)
        {
            repo = repository;
        }

        public IEnumerable<Player> GetLeaderboard(int page, int players)
        {
            return repo.GetAll().Where(p => p.IsActive).OrderByDescending(p => p.Wins).Skip(page * players).Take(players);
        }

        public ValidationResult<Player> Create(string name)
        {
            if (repo.GetByName(name) != null) return ValidationResult<Player>.Failure("User already exists");

            var random = new Random();

            var player = new Player
                             {
                                 Name = name,
                                 Id = Guid.NewGuid(),
                                 IsOnline = true,
                                 LastSeen = DateTime.Now,
                                 ApiKey = string.Format("{0}{1}{2}{3}{4}",
                                                        random.Next(0, 9),
                                                        random.Next(0, 9),
                                                        random.Next(0, 9),
                                                        random.Next(0, 9),
                                                        random.Next(0, 9)),
                                 IsActive = true
                             };

            repo.Add(player);
            repo.Save();

            return ValidationResult<Player>.Success.WithData(player);
        }

        public ValidationResult<Player> Login(string name, string apiKey)
        {
            Player player = repo.GetByName(name);

            // See if player 1) wasn't found or 2) ApiKey doesn't match
            if (player == null || string.CompareOrdinal(apiKey, player.ApiKey) != 0)
                return ValidationResult<Player>.Failure("User with specified key wasn't found.");

            player.IsOnline = true;

            return ValidationResult<Player>.Success.WithData(player);
        }

        public Player Get(Guid id)
        {
            return repo.Get(id);
        }

        public void Invite(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Ban(Guid id)
        {
            var player = repo.GetAll().FirstOrDefault(p => p.IsActive && p.Id == id);
            if (player != null) player.IsActive = false;
            repo.Save();
        }

        public IEnumerable<Player> Search(string searchTerm)
        {
            var players = repo.FindBy(p => p.IsActive && p.Name.Contains(searchTerm));

            return players;
        }
    }
}