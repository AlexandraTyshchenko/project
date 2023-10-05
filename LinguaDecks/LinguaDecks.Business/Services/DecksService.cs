using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Models;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Services
{
	public class DecksService : IDecksService
	{
		private readonly ApplicationContext _dbContext;
		private readonly IImageService _imageService;

		public DecksService(ApplicationContext dbContext, IImageService imageService)
		{
			_dbContext = dbContext;
			_imageService = imageService;
		}

		public async Task<Deck> GetDeckById(int id)
		{
			Deck deck = await _dbContext.Decks
				.Include(d => d.Author)
				.Include(d => d.Cards)
				.FirstAsync(d => d.Id == id);

			return deck;
		}

		public async Task<IEnumerable<Rating>> GetDecksRatings(int deckId)
		{
			List<Rating> ratings = await _dbContext.Ratings.Where(x => x.DeckId == deckId).ToListAsync();

			return ratings;
		}

		public async Task<IEnumerable<Comment>> GetDecksComments(int deckId)
		{
			List<Comment> comments = await _dbContext.Comments.Where(x => x.DeckId == deckId)
				.Include(c => c.User)
				.OrderBy(x => x.Date)
				.ToListAsync();

			return comments;
		}

		public async Task<IEnumerable<Card>> GetDeckCards(int deckId)
		{
			var properCards = await _dbContext.Cards.Where(x => x.DeckId == deckId).ToListAsync();

			return properCards;
		}

		public (float Rating, int Votes) CalculateDeckRating(IEnumerable<Rating> ratings)
		{
			return (ratings.Any() ? (float)ratings.Sum(rating => rating.Value) / ratings.Count() : 0,
				ratings.Any() ? ratings.Count() : 0);
		}

		public async Task SaveProgress(IEnumerable<CardProgress> cards)
		{
			foreach (var card in cards)
			{
				var сardProgress = await _dbContext.CardProgresses.FirstOrDefaultAsync(cp => cp.CardId == card.CardId && cp.UserId == card.UserId);

				if (сardProgress != null)
				{
					сardProgress.IsKnown = card.IsKnown;
				}
				else
				{
					await _dbContext.CardProgresses.AddAsync(card);
				}
			}

			await _dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<(int deckId, string iconPath)>> GetDeckIcons(IEnumerable<int> deckIds)
		{
			var icons = await _dbContext.Decks
				.Where(d => deckIds.Contains(d.Id))
				.Select(d => new { d.Id, d.IconPath })
				.ToListAsync();

			return icons.Select(d => (deckId: d.Id, iconPath: d.IconPath));
		}

		public async Task<Deck> CreateDeck(DeckCreateRequest request)
		{
			Deck deck = new Deck
			{
				Name = request.Name,
				Description = request.Description,
				PrimaryLanguage = request.PrimaryLanguage,
				SecondaryLanguage = request.SecondaryLanguage,
				AuthorId = request.AuthorId,
				IsVisible = true,
				CategoryId = request.CategoryId,
				IconPath = string.Empty
			};

			if (request.Icon != null)
			{
				Uri newIcon = await _imageService.SaveAsync(request.Icon);
				deck.IconPath = newIcon.AbsoluteUri;
			}

			await _dbContext.Decks.AddAsync(deck);
			await _dbContext.SaveChangesAsync();

			return deck;
		}

		public async Task UpdateDeck(int id, DeckUpdateRequest request)
		{
			Deck deck = await _dbContext.Decks.FindAsync(id);

			if (deck == null)
			{
				throw new NotFoundException($"{nameof(Deck)} with id {id} not found.");
			}

			deck.Name = request.Name;
			deck.Description = request.Description;
			deck.PrimaryLanguage = request.PrimaryLanguage;
			deck.SecondaryLanguage = request.SecondaryLanguage;
			deck.CategoryId = request.CategoryId;

			if (request.Icon != null)
			{
				if (!string.IsNullOrEmpty(deck.IconPath))
				{
					await _imageService.DeleteAsync(new Uri(deck.IconPath));
				}

				Uri newIcon = await _imageService.SaveAsync(request.Icon);
				deck.IconPath = newIcon.AbsoluteUri;
			}

			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteDeck(int id)
		{
			Deck deck = await _dbContext.Decks.FindAsync(id);

			if (deck == null)
			{
				throw new NotFoundException($"{nameof(Deck)} with id {id} not found.");
			}

			_dbContext.Decks.Remove(deck);
			await _dbContext.SaveChangesAsync();
		}
	}
}
