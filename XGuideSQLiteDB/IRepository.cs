using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XGuideSQLiteDB.Models;

namespace XGuideSQLiteDB
{
    public interface IRepository<T> where T : class, IEntity
    {
        void Create(T entity);

        void Delete(T entity);

        Task<bool> DeleteById(int id);

        List<T> Find(Func<T, bool> predicate);

        List<T> GetAll(IEnumerable<Expression<Func<T, object>>> includeProperty = null);

        Task<T> GetById(int id);

        Task<T> GetById(int id, IEnumerable<Expression<Func<T, object>>> includeProperty);

        void Update(T entity);
    }
}