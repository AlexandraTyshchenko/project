using LinguaDecks.DataAccess.Enums;
using System.Collections.Generic;

namespace LinguaDecks.DataAccess.Entities
{
	public class User
	{
		
		public int Id { get; set; }

		public string Email { get; set; }

		public string Name { get; set; }

		public Roles Role { get; set; }

		public string PasswordHash { get; set; }

		public bool IsBlocked { get; set; }

		public string IconPath { get; set; }

		public virtual IList<RefreshToken> Tokens { get; set; } = new List<RefreshToken>();

		public virtual IList<Deck> Decks { get; set; } = new List<Deck>();

		public virtual IList<CardProgress> Progresses { get; set; } = new List<CardProgress>();

		public virtual IList<Rating> Ratings { get; set; } = new List<Rating>();

		public virtual IList<Comment> Comments { get; set; } = new List<Comment>();

		public virtual TeacherRequest Request { get; set; }
	}
}
