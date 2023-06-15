using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace XGuideSQLiteDB
{
    public class Repository : IRepository
    {
        public Repository()
        {
        }

        public async void Create<T>(T entity) where T : class
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                db.Set<T>().Add(entity);
                await db.SaveChangesAsync();
            }
        }

        public async void Update<T>(T entity) where T : class
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        public async void Delete<T>(T entity) where T : class
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                db.Set<T>().Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteById<T>(int id) where T : class
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                T entity = await db.Set<T>().FindAsync(id);
                if (entity is null) return false;
                db.Set<T>().Remove(entity);
                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<T> GetById<T>(int id) where T : class
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                return await db.Set<T>().FindAsync(id);
            }
        }

        public List<T> GetAll<T>() where T : class
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                return db.Set<T>().ToList();
            }
        }

        public List<T> Find<T>(Func<T, bool> predicate) where T : class
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                return db.Set<T>().Where(predicate).ToList();
            }
        }
    }
}