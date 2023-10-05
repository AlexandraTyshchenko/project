using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Interfaces
{
	public interface ICommentService
	{
		Task AddComment(int userId, int deckId, string commentText);
	}
}
