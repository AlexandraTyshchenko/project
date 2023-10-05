using System.Collections.Generic;

namespace LinguaDecks.Api.Models
{
	public class UsersResponse
	{		
		public int TotalUsers { get; set; }

		public IEnumerable<SimpleUserModel> UserItems { get; set; } 
	}
}
