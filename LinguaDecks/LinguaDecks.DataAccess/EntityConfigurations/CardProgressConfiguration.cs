using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LinguaDecks.DataAccess.EntityConfigurations
{
	public class CardProgressConfiguration : IEntityTypeConfiguration<CardProgress>
	{
		public void Configure(EntityTypeBuilder<CardProgress> builder)
		{
			builder.ToTable("CardProgress").HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.HasOne(x => x.Card).WithMany(y => y.Progresses).HasForeignKey(x => x.CardId).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x => x.User).WithMany(y => y.Progresses).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.Property(x => x.IsKnown).IsRequired();
		}
	}
}
