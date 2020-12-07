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
			builder.Property(c => c.Id).ValueGeneratedNever();

			builder.Property(c => c.Name);

			builder.HasOne<Teacher>().WithMany(c => c.Classes).HasForeignKey(c => c.TeacherId);

		}
	}
}
