using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SamuraiServer.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Get(Guid id);
        void Add(T entity);
        void Delete(Guid id);
        void Edit(T entity);
        void Save();
    }
}
