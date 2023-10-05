using System.Collections.Generic;

namespace LinguaDecks.DataAccess.Entities
{
	public class Card
	{
		public int Id { get; set; }

		public string PrimaryText { get; set; }

		public string SecondaryText { get; set; }

		public int DeckId { get; set; }

		public virtual Deck Deck { get; set; }

		public virtual IList<CardProgress> Progresses { get; set; } = new List<CardProgress>();
	}
}
