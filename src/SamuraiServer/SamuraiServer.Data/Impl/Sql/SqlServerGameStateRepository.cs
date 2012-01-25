using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SamuraiServer.Data.Impl
{
    public class SqlServerGameStateRepository : IGameStateRepository
    {
        // TODO: We may also store our json in SQL nvarchar(MAX) if that is easier for people

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
    }
}
