using System.Collections.Generic;
using System.Threading.Tasks;
using LinguaDecks.Business.Models;
using LinguaDecks.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace LinguaDecks.Business.Interfaces
{
	public interface IDecksService
	{
		Task<Deck> GetDeckById(int id);

		Task<IEnumerable<Rating>> GetDecksRatings(int deckId);

		Task<IEnumerable<Comment>> GetDecksComments(int deckId);

		Task<IEnumerable<Card>> GetDeckCards(int deckId);

		(float Rating, int Votes) CalculateDeckRating(IEnumerable<Rating> ratings);

		Task SaveProgress(IEnumerable<CardProgress> cards);

		Task<IEnumerable<(int deckId, string iconPath)>> GetDeckIcons(IEnumerable<int> deckIds);

		Task<Deck> CreateDeck(DeckCreateRequest request);

		Task UpdateDeck(int id, DeckUpdateRequest request);

		Task DeleteDeck(int id);
	}
}
