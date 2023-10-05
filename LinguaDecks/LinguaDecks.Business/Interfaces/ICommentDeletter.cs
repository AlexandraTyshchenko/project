using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Interfaces
{
	public interface ICommentDeletter
	{
		Task DeleteComment(int commentId, int userId, int deckId);
	}
}
