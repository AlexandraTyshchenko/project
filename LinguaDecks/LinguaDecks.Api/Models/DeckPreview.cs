namespace LinguaDecks.Api.Models
{
	public class DeckPreview
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int CardsCount { get; set; }

		public string Author { get; set; }

		public double Rating { get; set; }

		public string Category { get; set; }
	}
}
