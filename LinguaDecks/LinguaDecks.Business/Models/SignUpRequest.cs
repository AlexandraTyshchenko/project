using LinguaDecks.DataAccess.Enums;

namespace LinguaDecks.Business.Models
{
	public class SignUpRequest
	{
		public string Name { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public Roles Role { get; set; }

		public string Description { get; set; }
	}
}
