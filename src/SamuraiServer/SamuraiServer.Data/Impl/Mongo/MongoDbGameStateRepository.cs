using System;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace SamuraiServer.Data.Impl
{
    public class MongoDbGameStateRepository : IGameStateRepository
    {
        private readonly MongoCollection<GameState> games;

        public MongoDbGameStateRepository(MongoCollection<GameState> games)
        {
            this.games = games;
        }

        public IQueryable<GameState> GetAll()
        {
            return games.FindAll().AsQueryable();
        }

        public IQueryable<GameState> FindBy(Expression<Func<GameState, bool>> predicate)
        {
            return games.FindAll().Where(predicate.Compile()).AsQueryable();
        }

        public GameState Get(Guid id)
        {
            var query = Query.EQ("Id", id);
            return games.Find(query).FirstOrDefault();
        }

        public void Add(GameState entity)
        {
            games.Save(entity);
        }

        public void Delete(Guid id)
        {
            var query = Query.EQ("Id", id); // TODO: confirm this is right query
            var sort = SortBy.Null;
            games.FindAndRemove(query, sort);
        }

        public void Edit(GameState entity)
        {
            games.Save(entity);
        }

        public void Save()
        {
            // no-op
        }

        public GameState GetByName(string name)
        {
            var query = Query.EQ("Name", name); // TODO: confirm this is right query
            return games.Find(query).FirstOrDefault();
        }
    }
}
