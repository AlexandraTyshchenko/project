using LinguaDecks.DataAccess.Enums;

namespace LinguaDecks.DataAccess.Entities
{
	public class TeacherRequest
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public virtual User User { get; set; }

		public string Description { get; set; }

		public Statuses Status { get; set; }
	}
}
