using LinguaDecks.DataAccess.Enums;

namespace LinguaDecks.Api.Models
{
	
	public class TeacherRequestModel
	{

		public int UserId { get; set; }

		public string Email { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public Statuses Status { get; set; }

	}
}
