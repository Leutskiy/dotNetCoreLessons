using EFCoreTutorial.Lesson_01.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreTutorial.Lesson_01.Infrastructure.Database.EntityConfigurations
{
	internal sealed class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
	{
		private readonly string Table = "Students";
		private readonly string Schema = "lesson_01";

		public void Configure(EntityTypeBuilder<Student> builder)
		{
			builder.ToTable(Table, Schema);

			builder.HasKey(s => s.Id);

			builder.Property(s => s.Name);

			builder.HasOne(s => s.Class).WithMany(t => t.Students);
		}
	}
}
