using LinguaDecks.DataAccess.Enums;
using static LinguaDecks.DataAccess.Entities.User;

namespace LinguaDecks.Api.Models
{
	public class TeacherModel
	{
		public int Id { get; set; }

		public Roles Role { get; set; }
	}
}
