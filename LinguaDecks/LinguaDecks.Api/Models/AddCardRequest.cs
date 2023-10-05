namespace LinguaDecks.Api.Models
{
	public class AddCardRequest
	{
		public int DeckId { get; set; }

		public string PrimaryText { get; set; }

		public string SecondaryText { get; set; }
	}
}
