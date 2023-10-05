namespace LinguaDecks.DataAccess.Entities
{
	public class CardProgress
	{
		public int Id { get; set; }

		public int CardId { get; set; }

		public virtual Card Card { get; set; }

		public int UserId { get; set; }

		public virtual User User { get; set; }

		public bool IsKnown { get; set; }
	}
}
