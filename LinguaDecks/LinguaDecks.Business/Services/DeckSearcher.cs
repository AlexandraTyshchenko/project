using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinguaDecks.Business.DTOs;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinguaDecks.Business.Services
{
	public class DeckSearcher : IDeckSearcher
	{
		private readonly ApplicationContext _dbContext;

		public DeckSearcher(ApplicationContext dbContext)
		{
			_dbContext = dbContext;
		}

		public int DecksCount { get; set; }

		public async Task<ICollection<Deck>> FindDeckAsync(int page, int pageSize, SearchParameters searchParameters)
		{
			IQueryable<Deck> query = _dbContext.Decks.Include(d => d.Author);

			if (!string.IsNullOrEmpty(searchParameters.Name))
			{
				query = query.Where(x => x.Name.StartsWith(searchParameters.Name));
			}

			if (!string.IsNullOrEmpty(searchParameters.PrimaryLanguage))
			{
				query = query.Where(x => x.PrimaryLanguage == searchParameters.PrimaryLanguage);
			}

			if (!string.IsNullOrEmpty(searchParameters.SecondaryLanguage))
			{
				query = query.Where(x => x.SecondaryLanguage == searchParameters.SecondaryLanguage);
			}

			if (searchParameters.CategoryId != 0)
			{
				query = query.Where(x => x.CategoryId == searchParameters.CategoryId);
			}

			if (!string.IsNullOrEmpty(searchParameters.Author))
			{
				query = query.Where(x => x.Author.Name.StartsWith(searchParameters.Author));
			}
			int startIndex = (page - 1) * pageSize;
			DecksCount = query.Count();
			var deckList = await query.Skip(startIndex).Take(pageSize).ToListAsync();
			return deckList;
		}
	}
}

