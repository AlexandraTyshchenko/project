using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinguaDecks.DataAccess.EntityConfigurations
{
	public class RatingConfiguration : IEntityTypeConfiguration<Rating>
	{
		public void Configure(EntityTypeBuilder<Rating> builder)
		{
			builder.ToTable("Rating").HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.HasOne(x => x.User).WithMany(y => y.Ratings).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.ClientSetNull);
			builder.HasOne(x => x.Deck).WithMany(y => y.Ratings).HasForeignKey(x => x.DeckId).OnDelete(DeleteBehavior.Cascade);
			builder.Property(x => x.Value).IsRequired();
		}
	}
}
