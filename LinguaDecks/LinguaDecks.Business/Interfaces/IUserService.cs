using LinguaDecks.Business.DTOs;
using LinguaDecks.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Interfaces
{
	public interface IUserService
	{
		Task<User> GetUser(int userId);

		Task<(int usersCount, IEnumerable<User> users)> FindUsers(int page, int pageSize, SearchUsersParameters searchParameters);
	}
}
