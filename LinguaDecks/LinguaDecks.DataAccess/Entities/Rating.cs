namespace LinguaDecks.DataAccess.Entities
{
	public class Rating
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public virtual User User { get; set; }

		public int DeckId { get; set; }

		public virtual Deck Deck { get; set; }

		public int Value { get; set; }
	}
}
