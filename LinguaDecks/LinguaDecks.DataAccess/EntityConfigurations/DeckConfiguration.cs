using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinguaDecks.DataAccess.EntityConfigurations
{
	public class DeckConfiguration : IEntityTypeConfiguration<Deck>
	{
		public void Configure(EntityTypeBuilder<Deck> builder)
		{
			builder.ToTable("Deck").HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
			builder.Property(x => x.PrimaryLanguage).IsRequired().HasMaxLength(50);
			builder.Property(x => x.SecondaryLanguage).IsRequired().HasMaxLength(50);
			builder.HasOne(x => x.Author).WithMany(y => y.Decks).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.SetNull);
			builder.Property(x => x.IsVisible).IsRequired();
			builder.HasOne(x => x.Category).WithMany(y => y.Decks).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
			builder.Property(x => x.IconPath).IsRequired().HasMaxLength(200);
		}
	}
}
