using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SamuraiServer.Data.Impl
{
    public class FileSystemGameStateRepository : IGameStateRepository
    {
        // TODO: Create a testing repo that serializes states to disk. This allows testing that can survive an app restart

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
