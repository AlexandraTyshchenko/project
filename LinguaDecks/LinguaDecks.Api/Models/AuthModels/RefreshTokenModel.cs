using System;

namespace LinguaDecks.Api.Models.AuthModels
{
	public class RefreshTokenModel
	{
		public int Id { get; set; }

		public SimpleUserModel User { get; set; }

		public string Value { get; set; }

		public DateTime ExpirationDate { get; set; }
	}
}
