using System.ComponentModel.DataAnnotations;

namespace LinguaDecks.Business.DTOs
{
	public class SearchParameters
	{
		[StringLength(50)]
		public string Name { get; set; }
		public string PrimaryLanguage { get; set; }
		public string SecondaryLanguage { get; set; }
		public int CategoryId { get; set; }

		[StringLength(50)]
		public string Author { get; set; }
	}

}
