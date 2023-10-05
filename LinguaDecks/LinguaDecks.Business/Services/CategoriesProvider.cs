using System.Collections.Generic;
using System.Threading.Tasks;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinguaDecks.Business.Services
{
	public class CategoriesProvider : ICategoriesProvider
	{
		private ApplicationContext _dbContext;

		public CategoriesProvider(ApplicationContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ICollection<Category>> GetCategoryAsync()
		{
			var result = await _dbContext.Categories.ToListAsync();
			return result;
		}
	}
}
