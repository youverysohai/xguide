using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XGuideSQLiteDB
{
    public interface IRepository
    {
        void Create<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> DeleteById<T>(int id) where T : class;

        List<T> Find<T>(Func<T, bool> predicate) where T : class;

        List<T> GetAll<T>() where T : class;

        Task<T> GetById<T>(int id) where T : class;

        void Update<T>(T entity) where T : class;
    }
}