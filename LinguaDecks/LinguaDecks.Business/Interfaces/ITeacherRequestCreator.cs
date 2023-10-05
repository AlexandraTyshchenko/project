using System.Threading.Tasks;
using LinguaDecks.DataAccess.Entities;

namespace LinguaDecks.Business.Interfaces
{
	public interface ITeacherRequestCreator
	{
		Task<TeacherRequest> CreateAsync(int userId, string description);
	}
}
