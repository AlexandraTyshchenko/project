using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinguaDecks.DataAccess.EntityConfigurations
{
	public class CardConfiguration : IEntityTypeConfiguration<Card>
	{
		public void Configure(EntityTypeBuilder<Card> builder)
		{
			builder.ToTable("Card").HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.Property(x => x.PrimaryText).IsRequired().HasMaxLength(50);
			builder.Property(x => x.SecondaryText).IsRequired().HasMaxLength(50);
			builder.HasOne(x => x.Deck).WithMany(y => y.Cards).HasForeignKey(x => x.DeckId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
