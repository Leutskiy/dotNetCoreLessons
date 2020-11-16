using EFCoreTutorial.Lesson_01.Domain;
using EFCoreTutorial.Lesson_01.Infrastructure.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new StudentEntityConfiguration());
			modelBuilder.ApplyConfiguration(new TeacherEntityConfiguration());
			modelBuilder.ApplyConfiguration(new ClassEntityConfiguration());
			modelBuilder.ApplyConfiguration(new SchoolEntityConfiguration());
		}
	}
}
