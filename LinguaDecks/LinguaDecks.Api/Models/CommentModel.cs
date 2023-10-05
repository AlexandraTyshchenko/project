using System;

namespace LinguaDecks.Api.Models
{
	public class CommentModel
	{
		public int Id { get; set; }

		public SimpleUserModel User { get; set; }

		public string Text { get; set; }

		public DateTime Date { get; set; }
	}
}
