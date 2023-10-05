using System.Collections.Generic;

namespace LinguaDecks.Api.Models.DeckModels
{
	public class DeckModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public IList<CardModel> Cards { get; set; }

		public string PrimaryLanguage { get; set; }

		public string SecondaryLanguage { get; set; }

		public SimpleUserModel Author { get; set; }

		public string IconPath { get; set; }

		public CategoryModel Category { get; set; }
	}
}
