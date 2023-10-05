using System.Collections.Generic;

namespace LinguaDecks.Api.Models
{
	public class DecksResponse
	{
		public IList<DeckPreview> DeckItems { get; set; } = new List<DeckPreview>();
		public int TotalDecks { get; set; }
	}
}
