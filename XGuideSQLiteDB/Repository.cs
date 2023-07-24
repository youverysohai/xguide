using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGuideSQLiteDB
{
    public class Repository : IRepository
    {
        private readonly ILogger _logger;

        public Repository(ILogger logger)
        {
            _logger = logger;
        }

        public async void Create<T>(T entity) where T : class
        {
            //TODO: Fix database append issue
            using (XGuideDbContext db = new XGuideDbContext())
            {
                db.ChangeTracker.TrackGraph(entity, node => node.Entry.State = !node.Entry.IsKeySet ? EntityState.Added : EntityState.Unchanged);

                await db.SaveChangesAsync();
                _logger.Information($"Added {entity.ToString()}");
            }
        }

        public async void Update<T>(T entity) where T : class
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                _logger.Information($"Updated {entity.ToString()}");
            }
        }

        public async void Delete<T>(T entity) where T : class
        {
            using (XGuideDbContext db = new XGuideDbContext())
            {
                db.Set<T>().Remove(entity);
                await db.SaveChangesAsync();

                _logger.Information($"Deleted {entity.ToString()}");
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
                _logger.Information($"Deleted {entity.ToString()}");
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