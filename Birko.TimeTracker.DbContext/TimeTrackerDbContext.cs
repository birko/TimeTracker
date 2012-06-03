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
            //Tag
            modelBuilder.Entity<Birko.TimeTracker.Entities.Tag>().HasKey(e => e.ID);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Tag>().Property(e => e.ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Tag>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Birko.TimeTracker.Entities.Tag>().HasMany<Birko.TimeTracker.Entities.Task>(e => e.Tasks).WithMany(t=>t.Tags);
            //Task
            modelBuilder.Entity<Birko.TimeTracker.Entities.Task>().HasKey(e => e.ID);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Task>().Property(e => e.ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Task>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Birko.TimeTracker.Entities.Task>().HasOptional(e => e.Category).WithMany(c => c.Tasks).HasForeignKey(e => e.CategoryID);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Task>().HasMany<Birko.TimeTracker.Entities.Tag>(e => e.Tags).WithMany(t =>t.Tasks);
            //Category
            modelBuilder.Entity<Birko.TimeTracker.Entities.Category>().HasKey(e => e.ID);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Category>().Property(e => e.ID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Birko.TimeTracker.Entities.Category>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Birko.TimeTracker.Entities.Category>().Property(e => e.Group).IsOptional();
            modelBuilder.Entity<Birko.TimeTracker.Entities.Category>().HasMany<Birko.TimeTracker.Entities.Task>(e => e.Tasks).WithOptional(t => t.Category).HasForeignKey(i => i.CategoryID);
        }
    }
}
