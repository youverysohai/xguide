using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XGuideSQLiteDB.Models;

namespace XGuideSQLiteDB
{

    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        public async void Create(T entity)
        {
            //TODO: Fix database append issue
            using (XGuideDbContext db = new XGuideDbContext())
            {
                db.ChangeTracker.TrackGraph(entity, node => node.Entry.State = !node.Entry.IsKeySet ? EntityState.Added : EntityState.Unchanged);

                await db.SaveChangesAsync();

            }
        }

        public async void Update(T entity)
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        public async void Delete(T entity)
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                db.Set<T>().Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteById(int id)
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

        public List<T> GetAll()
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                return db.Set<T>().ToList();
            }
        }

        public async Task<T> GetById(int id)
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                return await db.Set<T>().FindAsync(id);
            }
        }

        public async Task<T> GetById(int id, IEnumerable<Expression<Func<T, object>>> includeProperty)
        {
            return await Task.Run(() =>
            {
                var data = GetAll(includeProperty);
                return data.FirstOrDefault(x => x.Id == id);
            });
        }

        public List<T> GetAll(IEnumerable<Expression<Func<T, object>>> includeProperty)
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                var query = db.Set<T>().AsQueryable();
                if (includeProperty != null)
                {
                    foreach (var property in includeProperty)
                    {
                        query = query.Include(property);
                    }
                }
                return query.ToList();
            }
        }

        //A better design would be to introduce an interface, say IEntity, that all your entity classes implement.This interface could have a property Id and you could replace T with T : IEntity in your generic repository methods

        public List<T> Find(Func<T, bool> predicate)
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                return db.Set<T>().Where(predicate).ToList();
            }
        }
    }
}