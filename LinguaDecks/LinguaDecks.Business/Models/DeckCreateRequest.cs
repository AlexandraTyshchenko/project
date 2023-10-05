using Microsoft.AspNetCore.Http;

namespace LinguaDecks.Business.Models
{
	public class DeckCreateRequest
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string PrimaryLanguage { get; set; }

		public string SecondaryLanguage { get; set; }

		public int CategoryId { get; set; }

		public int AuthorId { get; set; }

		public IFormFile Icon { get; set; }
	}
}
