using System.Collections.Generic;

namespace LinguaDecks.Api.Models
{
	public class DeckProgressModel
	{
		public int DeckId { get; set; }

		public string DeckName { get; set; }

		public ProgressModel Progress { get; set; }
	}
}
