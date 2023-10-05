using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Services
{
	public class CategoriesCreator : ICategoriesCreator
	{
		private readonly ApplicationContext _dbContext;

		public CategoriesCreator(ApplicationContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task CreateCategory(string categoryName)
		{
			Category category = new Category() { Name = categoryName };

			var foundCategory = _dbContext.Categories.FirstOrDefault(c => c.Name == categoryName);
			if (foundCategory != null)
			{
				throw new BadRequestException("Category already exists");
			}
			await _dbContext.Categories.AddAsync(category);
			_dbContext.SaveChanges();
		}
	}
}
