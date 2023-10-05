using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace LinguaDecks.DataAccess.Context
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext()
		{
		}

		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
		}

		public virtual DbSet<Card> Cards { get; set; }

		public virtual DbSet<CardProgress> CardProgresses { get; set; }

		public virtual DbSet<Category> Categories { get; set; }

		public virtual DbSet<Comment> Comments { get; set; }

		public virtual DbSet<Deck> Decks { get; set; }

		public virtual DbSet<Rating> Ratings { get; set; }

		public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

		public virtual DbSet<TeacherRequest> TeacherRequests { get; set; }

		public virtual DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CardConfiguration());
			modelBuilder.ApplyConfiguration(new CardProgressConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryConfiguration());
			modelBuilder.ApplyConfiguration(new CommentConfiguration());
			modelBuilder.ApplyConfiguration(new DeckConfiguration());
			modelBuilder.ApplyConfiguration(new RatingConfiguration());
			modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
			modelBuilder.ApplyConfiguration(new TeacherRequestConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
		}
	}
}
