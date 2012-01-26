using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SamuraiServer.Data.Impl
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private static List<Player> _players = new List<Player>();
        
        public dynamic Context { get { return _players; } }

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