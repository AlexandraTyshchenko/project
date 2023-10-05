using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Services
{
	public class CommentService : ICommentService
	{
		private readonly ApplicationContext _dbContext;

		public CommentService(ApplicationContext dbContext)
		{
			_dbContext= dbContext;
		}

		public async Task AddComment(int userId, int deckId, string commentText)
		{
			var deck = await _dbContext.Decks.FindAsync(deckId);
			if (deck == null)
			{
				throw new NotFoundException("deck is not found");
			}//is not necessary to check whether the user exists as they must be authorized
			var comment = new Comment
			{
				UserId = userId,
				DeckId = deckId,
				Text = commentText,
				Date = DateTime.UtcNow
			};
			await  _dbContext.Comments.AddAsync(comment);
			await _dbContext.SaveChangesAsync();
		}

	}
}
