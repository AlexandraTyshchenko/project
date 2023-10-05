using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Services
{
	public class CommentDeletter:ICommentDeletter
	{
		private readonly ApplicationContext _dbContext;

		public CommentDeletter(ApplicationContext dbContext)
		{
			_dbContext= dbContext;
		}

		public async Task DeleteComment(int commentId,int userId,int deckId)
		{
			var comment = await  _dbContext.FindAsync<Comment>(commentId);
			var user = await _dbContext.FindAsync<User>(userId);
			var deck = await _dbContext.FindAsync<Deck>(deckId);

			if (comment==null || deck == null)
			{
				throw new NotFoundException();
			}

			else if(user.Role == Roles.Student && comment.UserId==userId)
			{
				_dbContext.Comments.Remove(comment);
			}

			else if (user.Role == Roles.Teacher && deck.AuthorId == userId || comment.UserId == userId)
			{
				_dbContext.Comments.Remove(comment);
			}

			else if (user.Role == Roles.Admin || comment.UserId == userId)
			{
				_dbContext.Comments.Remove(comment);
			}

			else
			{
				throw new UnauthorizedAccessException();
			}
			await _dbContext.SaveChangesAsync();
		}
	}
}
