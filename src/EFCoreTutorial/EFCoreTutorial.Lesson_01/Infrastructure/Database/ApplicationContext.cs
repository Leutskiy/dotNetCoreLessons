using EFCoreTutorial.Lesson_01.Domain;
using EFCoreTutorial.Lesson_01.Infrastructure.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace EFCoreTutorial.Lesson_01.Infrastructure.Database
{
	public sealed class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Class> Classes { get; set; }
		public DbSet<School> Schools { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Teacher> Teachers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
		{
			dbContextOptionsBuilder.UseLoggerFactory(new LoggerFactory());
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new StudentEntityConfiguration());
			modelBuilder.ApplyConfiguration(new TeacherEntityConfiguration());
			modelBuilder.ApplyConfiguration(new ClassEntityConfiguration());
			modelBuilder.ApplyConfiguration(new SchoolEntityConfiguration());
		}

		public override int SaveChanges()
		{
			var modifiedEntries = this.ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Modified)
				.ToArray();

			var addedEntries = this.ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Added)
				.ToArray();

			

			return base.SaveChanges();	
		}
	}
}
