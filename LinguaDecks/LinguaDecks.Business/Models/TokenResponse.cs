using LinguaDecks.DataAccess.Entities;

namespace LinguaDecks.Business.Models
{
	public class TokenResponse
	{
		public string Token { get; set; }

		public RefreshToken RefreshToken { get; set; }
	}
}
