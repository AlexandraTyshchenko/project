using LinguaDecks.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Interfaces
{
	public interface IProgressService
	{
		Task<Dictionary<int, IEnumerable<CardProgress>>> GetTotalProgress(int userId);

		Task<IEnumerable<CardProgress>> GetDeckProgress(int userId, int deckId); 
		
		Task<(int positive, int negative, int total)> CalculateProgress(int deckId, IEnumerable<CardProgress> progresses);
	}
}
