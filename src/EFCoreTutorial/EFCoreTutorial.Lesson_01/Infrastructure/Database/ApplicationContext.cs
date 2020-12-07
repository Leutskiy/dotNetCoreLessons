using EFCoreTutorial.Lesson_01.Domain;
using EFCoreTutorial.Lesson_01.Infrastructure.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace EFCoreTutorial.Lesson_01.Infrastructure.Database
{
	public sealed class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Class> Classes { get; set; }

		public DbSet<Teacher> Teachers { get; set; }

		// Different cinfigurations such as Database Connection or Database Driver Logging
		protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
		{
			dbContextOptionsBuilder.UseLoggerFactory(new LoggerFactory());
		}

		// Data Model
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema("notifications");

			modelBuilder.ApplyConfiguration(new TeacherEntityConfiguration());
			modelBuilder.ApplyConfiguration(new ClassEntityConfiguration());

			ChangeTracker.Tracked += OnEntityCreated;
			ChangeTracker.StateChanged += OnEntityChanged;

			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges()
		{
			var modifiedEntries = ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Modified)
				.ToArray();

			var addedEntries = ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Added)
				.ToArray();

			return base.SaveChanges();
		}

		void OnEntityCreated(object sender, EntityTrackedEventArgs e)
		{
			if (!e.FromQuery && e.Entry.State == EntityState.Added && e.Entry.Entity is DomainEntity entity)
            {
			}
		}

		void OnEntityChanged(object sender, EntityStateChangedEventArgs e)
		{
			if (e.NewState == EntityState.Modified && e.Entry.Entity is DomainEntity entity)
			{
			}
		}
	}
}
