using System.Threading.Tasks;
using LinguaDecks.Business.Models;
using LinguaDecks.DataAccess.Entities;

namespace LinguaDecks.Business.Interfaces
{
	public interface IAuthenticationService
    {
		Task<User> SignInAsync(SignInRequest request);
		Task<User> SignUpAsync(SignUpRequest request);
	}
}
