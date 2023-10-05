using System.Collections.Generic;

namespace LinguaDecks.Api.Models
{
	public class SaveProgressRequest
	{
		public int DeckId { get; set; }
		public int UserId { get; set; }
		public IEnumerable<CardAnswerModel> Answers { get; set; }
	}
}
