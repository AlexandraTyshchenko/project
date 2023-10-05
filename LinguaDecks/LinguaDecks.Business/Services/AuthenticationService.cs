using System.Threading.Tasks;
using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Models;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using LinguaDecks.DataAccess.Services;
using Microsoft.EntityFrameworkCore;

namespace LinguaDecks.Business.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly ApplicationContext _context;

		public AuthenticationService(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<User> SignInAsync(SignInRequest request)
		{
			User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

			if (user == null)
			{
				throw new NotFoundException($"{nameof(User)} with email {request.Email} not found.");
			}

			if (!new IdentityPasswordHasher().VerifyPassword(user.PasswordHash, request.Password))
			{
				throw new BadRequestException("Invalid password.");
			}

			return user;
		}

		public async Task<User> SignUpAsync(SignUpRequest request)
		{
			User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

			if (user != null)
			{
				throw new BadRequestException($"{nameof(User)} with email {request.Email} already exists.");
			}

			user = new User
			{
				Name = request.Name,
				Email = request.Email,
				PasswordHash = new IdentityPasswordHasher().HashPassword(request.Password),
				Role = request.Role == Roles.Teacher? Roles.Student : request.Role,
				IsBlocked = false,
				IconPath = string.Empty
			};
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			return user;
		}
	}
}
