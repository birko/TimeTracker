using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Birko.TimeTracker.Entities;

namespace Birko.TimeTracker.DbContext
{
    public class TimeTrackerDbContext : System.Data.Entity.DbContext
    {
        public DbSet<Birko.TimeTracker.Entities.Category> Categories { get; set; }
        public DbSet<Birko.TimeTracker.Entities.Task> Tasks { get; set; }
        public DbSet<Birko.TimeTracker.Entities.Tag> Tags { get; set; }

        public TimeTrackerDbContext()
            :base()
        {
            Database.SetInitializer<TimeTrackerDbContext>(new CreateDatabaseIfNotExists<TimeTrackerDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Tag>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Birko.TimeTracker.Entities.Task>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Birko.TimeTracker.Entities.Category>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Birko.TimeTracker.Entities.Category>().Property(e => e.Group).IsOptional();
            modelBuilder.Entity<Birko.TimeTracker.Entities.Category>().HasMany<Birko.TimeTracker.Entities.Task>(e => e.Tasks);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Tag>().HasMany<Birko.TimeTracker.Entities.Task>(e => e.Tasks);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Task>().HasMany<Birko.TimeTracker.Entities.Tag>(e => e.Tags);
        }
    }
}
