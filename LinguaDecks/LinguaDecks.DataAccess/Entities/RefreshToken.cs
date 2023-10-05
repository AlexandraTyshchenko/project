using System;

namespace LinguaDecks.DataAccess.Entities
{
	public class RefreshToken
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public virtual User User { get; set; }

		public string Value { get; set; }

		public DateTime ExpirationDate { get; set; }
	}
}
