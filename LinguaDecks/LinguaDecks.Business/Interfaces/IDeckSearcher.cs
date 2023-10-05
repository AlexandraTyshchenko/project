using System.Collections.Generic;
using System.Threading.Tasks;
using LinguaDecks.Business.DTOs;
using LinguaDecks.DataAccess.Entities;

namespace LinguaDecks.Business.Interfaces
{
	public interface IDeckSearcher
    {
		Task<ICollection<Deck>> FindDeckAsync(int page,int pageSize,SearchParameters searchParameters);
		public int DecksCount { get; set; }

	}
}
