using LinguaDecks.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace LinguaDecks.Business.DTOs
{
	public class SearchUsersParameters
	{
		[StringLength(50)]
		public string Name { get; set; }

		public string Email { get; set; }

		public Roles? Role { get; set; }
	}
}
