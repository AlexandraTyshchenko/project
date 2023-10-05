using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Services
{
	public class CardsService : ICardsService
	{
		private readonly ApplicationContext _context;

		public CardsService(ApplicationContext context)
		{
			_context = context;
		}

		public async Task AddCard(Card card)
		{
			var deck = await _context.Decks.FindAsync(card.DeckId);
			if ( !string.IsNullOrEmpty(card.PrimaryText) && !string.IsNullOrEmpty(card.SecondaryText) && deck != null)
			{
				await _context.Cards.AddAsync(card);

				_context.SaveChanges();
			}
		}

		public async Task DeleteCard(int cardId)
		{
			Card card = await _context.Cards.FindAsync(cardId);
			_context.Cards.Remove(card);

			_context.SaveChanges();
		}
	}
}
