using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SamuraiServer.Data
{
    public interface IGenericRepository<T, in I> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Get(I id);
        void Add(T entity);
        void Delete(I id);
        void Edit(T entity);
        void Save();
    }
}
