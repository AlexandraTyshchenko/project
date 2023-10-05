using System.Collections.Generic;

namespace LinguaDecks.Api.Models.DeckModels
{
	public class DeckResponse
	{
		public DeckModel Deck { get; set; }

		public IEnumerable<CommentModel> Comments { get; set; }

		public DeckRatingModel DeckRating { get; set; }
	}
}
