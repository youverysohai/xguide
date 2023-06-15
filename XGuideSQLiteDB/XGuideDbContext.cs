using SQLite.CodeFirst;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SQLite.EF6.Migrations;
using XGuideSQLiteDB.Models;

namespace XGuideSQLiteDB
{
    public class XGuideDbContext : DbContext
    {
        static XGuideDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<XGuideDbContext, ContextMigrationConfiguration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<XGuideDbContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Calibration> Calibrations { get; set; }
        public DbSet<Manipulator> Manipulators { get; set; }
    }

    public sealed class ContextMigrationConfiguration : DbMigrationsConfiguration<XGuideDbContext>
    {
        public ContextMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }
    }
}