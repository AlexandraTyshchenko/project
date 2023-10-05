using LinguaDecks.DataAccess.Entities;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Interfaces
{
	public interface ICardsService
	{
		Task AddCard(Card card);
		Task DeleteCard(int cardId);
	}
}
