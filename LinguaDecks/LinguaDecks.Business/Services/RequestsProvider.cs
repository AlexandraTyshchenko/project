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
	public class RequestsProvider : IRequestsProvider
	{
		private readonly ApplicationContext _dbContext;

		public RequestsProvider(ApplicationContext dbContext)
		{
			_dbContext = dbContext;
		}

		public int TotalRequestsCount { get ; set; }

		public async Task<IEnumerable<TeacherRequest>> GetTeacherRequests(int page, int pageSize, Statuses status)
		{
			int startIndex = (page - 1) * pageSize;

			var teacherRequestsQuery = _dbContext.TeacherRequests
				.Include(x => x.User)
				.Where(x => x.Status == status);

			TotalRequestsCount = await teacherRequestsQuery.CountAsync();

			var teacherRequests = await teacherRequestsQuery
				.Skip(startIndex)
				.Take(pageSize)
				.ToListAsync();

			return teacherRequests;
		}
	}
}
