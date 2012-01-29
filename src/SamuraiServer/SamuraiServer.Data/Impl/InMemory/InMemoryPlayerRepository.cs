using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SamuraiServer.Data.Impl
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private static List<Player> _players = new List<Player>();

        public dynamic Context { get { return _players; } }

        public InMemoryPlayerRepository()
        {
            if (_players.Count == 0)
            {
                _players.Add(new Player
                {
                    Id = Guid.NewGuid(),
                    IsActive = false,
                    IsOnline = true,
                    LastSeen = DateTime.Now.Subtract(TimeSpan.FromHours(24)),
                    Name = "shiftkey",
                    GamesPlayed = 20,
                    Wins = 10
                });
                _players.Add(new Player
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    IsOnline = true,
                    LastSeen = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
                    Name = "aeoth",
                    GamesPlayed = 50,
                    Wins = 40
                });
                _players.Add(new Player
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    IsOnline = false,
                    LastSeen = DateTime.Now.Subtract(TimeSpan.FromHours(48)),
                    Name = "tobin",
                    GamesPlayed = 25,
                    Wins = 15
                });
                _players.Add(new Player
                {
                    Id = Guid.NewGuid(),
                    IsActive = true,
                    IsOnline = false,
                    LastSeen = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
                    Name = "csainty",
                    GamesPlayed = 1,
                    Wins = 0,
                    ApiKey = "12345"
                });
            }
        }

        public IQueryable<Player> GetAll()
        {
            return _players.AsQueryable();
        }

        public IQueryable<Player> FindBy(Expression<Func<Player, bool>> predicate)
        {
            return _players.AsQueryable().Where(predicate);
        }

        public Player Get(Guid id)
        {
            return _players.FirstOrDefault(p => p.Id == id);
        }

        public Player GetByName(string name)
        {
            return _players.FirstOrDefault(p => p.Name == name);
        }

        public void Add(Player entity)
        {
            _players.Add(entity);
        }

        public void Delete(Guid id)
        {
            var player = _players.FirstOrDefault(p => p.Id == id);
            _players.Remove(player);
        }

        public void Edit(Player entity)
        {
            var player = _players.FirstOrDefault(p => p.Id == entity.Id);
            if (player != null)
            {
                player.IsActive = entity.IsActive;
                player.IsOnline = entity.IsOnline;
                player.LastSeen = entity.LastSeen;
                player.Name = entity.Name;
                player.Wins = entity.Wins;
                player.GamesPlayed = entity.GamesPlayed;
            }
        }

        public void Save()
        {
        }
    }
}