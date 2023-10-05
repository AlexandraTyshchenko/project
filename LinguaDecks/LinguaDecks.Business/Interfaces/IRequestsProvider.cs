using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Interfaces
{
	public interface IRequestsProvider
	{
		Task<IEnumerable<TeacherRequest>> GetTeacherRequests(int page, int pageSize, Statuses status);
		public int TotalRequestsCount { get; set; }
	}
}
