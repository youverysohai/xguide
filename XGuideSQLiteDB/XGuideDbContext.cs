using Microsoft.EntityFrameworkCore;
using System;
using XGuideSQLiteDB.Models;

namespace XGuideSQLiteDB
{
    public class XGuideDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Calibration> Calibrations { get; set; }
        public DbSet<Manipulator> Manipulators { get; set; }

        public string DbPath { get; }

        public XGuideDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "blogging.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}