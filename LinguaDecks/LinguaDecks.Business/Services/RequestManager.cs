using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Services
{
	public class RequestManager : IRequestManager
	{
		private readonly ApplicationContext _dbContext;

		public RequestManager(ApplicationContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AnswerRequest(int id, bool answer)
		{
			
				var request = await _dbContext.TeacherRequests.FirstOrDefaultAsync(x => x.UserId == id);

				if (request == null)
				{
					throw new NotFoundException();
				}
				var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
				if (answer)
				{
					request.Status = Statuses.Approved;
					user.Role = Roles.Teacher;
				}
				else
				{
					request.Status = Statuses.Rejected;
					user.Role = Roles.Student; //user may be rejected after acceptance, that`s why we need to change the role
				}
				await _dbContext.SaveChangesAsync();
		}
	}
}
