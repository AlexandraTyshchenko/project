using System.Threading.Tasks;
using LinguaDecks.Business.Models;
using LinguaDecks.DataAccess.Entities;

namespace LinguaDecks.Business.Interfaces
{
	public interface ITokenService
	{
		Task<TokenResponse> CreateTokenAsync(User user);
		Task<TokenResponse> RefreshTokenAsync(string expiredToken);
	}
}
