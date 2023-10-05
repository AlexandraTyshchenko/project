using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinguaDecks.DataAccess.EntityConfigurations
{
	public class CommentConfiguration : IEntityTypeConfiguration<Comment>
	{
		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			builder.ToTable("Comment").HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedOnAdd();
			builder.HasOne(x => x.User).WithMany(y => y.Comments).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x => x.Deck).WithMany(y => y.Comments).HasForeignKey(x => x.DeckId).OnDelete(DeleteBehavior.Cascade);
			builder.Property(x => x.Text).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Date).IsRequired();
		}
	}
}
