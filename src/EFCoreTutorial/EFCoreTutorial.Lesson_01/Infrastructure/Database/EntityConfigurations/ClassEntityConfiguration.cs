using EFCoreTutorial.Lesson_01.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreTutorial.Lesson_01.Infrastructure.Database.EntityConfigurations
{
	internal sealed class ClassEntityConfiguration : IEntityTypeConfiguration<Class>
	{
		private readonly string Table = "Classes";
		private readonly string Schema = "lesson_01";

		public void Configure(EntityTypeBuilder<Class> builder)
		{
			builder.ToTable(Table, Schema);

			builder.HasKey(c => c.Id);

			builder.Property(c => c.Name);

			builder.HasOne(c => c.Teacher).WithMany(t => t.Classes);
			builder.HasMany(c => c.Students).WithOne(s => s.Class);
		}
	}
}
