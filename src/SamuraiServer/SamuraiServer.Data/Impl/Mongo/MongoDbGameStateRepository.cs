using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SamuraiServer.Data.Impl
{
    public class MongoDbGameStateRepository : IGameStateRepository
    {
        // TODO: If we decide to launch with MongoDb then we need to be able to store our game state

        public IQueryable<GameState> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<GameState> FindBy(Expression<Func<GameState, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public GameState Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Add(GameState entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Edit(GameState entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public GameState GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
