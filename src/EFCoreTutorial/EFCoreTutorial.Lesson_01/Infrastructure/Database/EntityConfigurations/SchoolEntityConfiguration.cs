using EFCoreTutorial.Lesson_01.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreTutorial.Lesson_01.Infrastructure.Database.EntityConfigurations
{
	internal sealed class SchoolEntityConfiguration : IEntityTypeConfiguration<School>
	{
		private readonly string Table = "Schools";
		private readonly string Schema = "lesson_01";

		public void Configure(EntityTypeBuilder<School> builder)
		{
			builder.ToTable(Table, Schema);

			builder.HasKey(s => s.Id);

			builder.Property(s => s.Name);
			builder.Property(s => s.City);
			builder.Property(s => s.State);

			builder.HasMany(s => s.Teachers).WithOne(t => t.School);
		}
	}
}
