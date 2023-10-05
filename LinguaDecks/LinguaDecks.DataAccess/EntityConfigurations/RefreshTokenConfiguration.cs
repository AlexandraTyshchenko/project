using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinguaDecks.DataAccess.EntityConfigurations
{
	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.ToTable("RefreshToken").HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.HasOne(x => x.User).WithMany(y => y.Tokens).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.Property(x => x.Value).IsRequired();
			builder.Property(x => x.ExpirationDate).IsRequired();
		}
	}
}
