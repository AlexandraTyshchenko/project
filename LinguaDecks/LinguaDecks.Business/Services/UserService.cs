using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinguaDecks.Business.DTOs;
using System.Linq;
using LinguaDecks.DataAccess.Enums;
using System;

namespace LinguaDecks.Business.Services
{
	public class UserService : IUserService
	{
		private readonly ApplicationContext _context;

		public UserService(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<User> GetUser(int userId)
		{
			var result = await _context.Users.FindAsync(userId);

			if(result == null)
			{
				throw new NotFoundException($"User with id {userId} not found");
			}

			return result;
		}
		
		public async Task<(int usersCount, IEnumerable<User> users)> FindUsers(int page, int pageSize, SearchUsersParameters searchParameters)
		{
			IQueryable<User> query = _context.Users;

			if (!string.IsNullOrEmpty(searchParameters.Name))
			{
				query = query.Where(x => x.Name.Contains(searchParameters.Name));
			}

			if (!string.IsNullOrEmpty(searchParameters.Email))
			{
				query = query.Where(x => x.Email.Contains(searchParameters.Email));
			}

			if (searchParameters.Role.HasValue && Enum.IsDefined(typeof(Roles), searchParameters.Role))
			{
				query = query.Where(x => x.Role == searchParameters.Role.Value);
			}

			int startIndex = (page - 1) * pageSize;
			int usersCount = query.Count();
			var userList = await query.Skip(startIndex).Take(pageSize).ToListAsync();

			return (usersCount, userList);
		}
	}
}
