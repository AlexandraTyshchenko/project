using Microsoft.AspNetCore.Http;

namespace LinguaDecks.Business.Models
{
	public class DeckUpdateRequest
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string PrimaryLanguage { get; set; }

		public string SecondaryLanguage { get; set; }

		public int CategoryId { get; set; }

		public IFormFile Icon { get; set; }
	}
}
