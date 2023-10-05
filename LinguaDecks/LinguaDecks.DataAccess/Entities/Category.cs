using System.Collections.Generic;

namespace LinguaDecks.DataAccess.Entities
{
    public class Category
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public virtual IList<Deck> Decks { get; set; } = new List<Deck>();
	}
}
