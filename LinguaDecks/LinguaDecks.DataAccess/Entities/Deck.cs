using System.Collections.Generic;

namespace LinguaDecks.DataAccess.Entities
{
	public class Deck
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public virtual IList<Card> Cards { get; set; } = new List<Card>();

		public string PrimaryLanguage { get; set; }

		public string SecondaryLanguage { get; set; }

		public int? AuthorId { get; set; }

		public virtual User Author { get; set; }

		public bool IsVisible { get; set; }

		public int? CategoryId { get; set; }

		public virtual Category Category { get; set; }

		public string IconPath { get; set; }

		public virtual IList<Rating> Ratings { get; set; } = new List<Rating>();

		public virtual IList<Comment> Comments { get; set; } = new List<Comment>();
	}
}
