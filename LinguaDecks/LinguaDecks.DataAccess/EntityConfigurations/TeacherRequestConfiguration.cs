using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinguaDecks.DataAccess.EntityConfigurations
{
	public class TeacherRequestConfiguration : IEntityTypeConfiguration<TeacherRequest>
	{
		public void Configure(EntityTypeBuilder<TeacherRequest> builder)
		{
			builder.ToTable("TeacherRequest").HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.Property(x => x.Description).IsRequired().HasMaxLength(300);
			builder.Property(x => x.Status).IsRequired();
		}
	}
}
