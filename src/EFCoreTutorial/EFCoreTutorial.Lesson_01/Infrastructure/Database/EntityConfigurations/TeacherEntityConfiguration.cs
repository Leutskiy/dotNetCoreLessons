using EFCoreTutorial.Lesson_01.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreTutorial.Lesson_01.Infrastructure.Database.EntityConfigurations
{
	internal sealed class TeacherEntityConfiguration : IEntityTypeConfiguration<Teacher>
	{
		private readonly string Table = "Teachers";
		private readonly string Schema = "lesson_01";

		public void Configure(EntityTypeBuilder<Teacher> builder)
		{
			builder.ToTable(Table, Schema);

			builder.HasKey(t => t.Id);

			builder.Property(t => t.Name);

			builder.HasOne(t => t.School).WithMany(s => s.Teachers);
			builder.HasMany(t => t.Classes).WithOne(c => c.Teacher);
		}
	}
}
