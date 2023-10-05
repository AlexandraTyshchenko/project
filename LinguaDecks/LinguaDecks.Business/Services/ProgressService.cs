using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Services
{
	public class ProgressService : IProgressService
	{
		private readonly ApplicationContext _context;

		public ProgressService(ApplicationContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<CardProgress>> GetDeckProgress(int userId, int deckId)
		{
			var deckCards = _context.Cards
				.Where(c => c.DeckId == deckId)
				.Select(c => c.Id);

			var userProgress = _context.CardProgresses.Where(cp => cp.UserId == userId && deckCards.Contains(cp.CardId));

			return await userProgress.ToListAsync();
		}

		public async Task<Dictionary<int, IEnumerable<CardProgress>>> GetTotalProgress(int userId)
		{
			var userProgress = await _context.CardProgresses
				.Include(cp => cp.Card)
				.Where(cp => cp.UserId == userId)
				.ToListAsync();

			var groupedProgress = userProgress.GroupBy(up => up.Card.DeckId);
			Dictionary<int, IEnumerable<CardProgress>> totalProgress = new();

			foreach (var progress in groupedProgress)
			{
				totalProgress.Add(progress.Key, progress);
			}

			return totalProgress;
		}

		public async Task<(int positive, int negative, int total)> CalculateProgress(int deckId, IEnumerable<CardProgress> progresses)
		{
			Deck deck = await _context.Decks.Include(d => d.Cards).FirstAsync(d => d.Id == deckId);
			int total = deck.Cards.Count;
			int positive = progresses.Count(p => p.IsKnown);
			int negative = progresses.Count(p => !p.IsKnown);

			return (positive, negative, total);
		}
	}
}
