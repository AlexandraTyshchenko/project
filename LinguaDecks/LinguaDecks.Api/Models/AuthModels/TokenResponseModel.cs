namespace LinguaDecks.Api.Models.AuthModels
{
	public class TokenResponseModel
	{
		public string Token { get; set; }

		public RefreshTokenModel RefreshToken { get; set; }
	}
}
