using System.Threading.Tasks;
using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace LinguaDecks.Business.Services
{
	public class TeacherRequestCreator : ITeacherRequestCreator
	{
		private readonly ApplicationContext _context;

		public TeacherRequestCreator(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<TeacherRequest> CreateAsync(int userId, string description)
		{
			TeacherRequest request = await _context.TeacherRequests.FirstOrDefaultAsync(r => r.UserId == userId);

			if (request != null)
			{
				throw new BadRequestException("Request already exists.");
			}

			request = new TeacherRequest
			{
				UserId = userId,
				Description = description,
				Status = Statuses.Pending
			};
			await _context.TeacherRequests.AddAsync(request);
			await _context.SaveChangesAsync();

			return request;
		}
	}
}
