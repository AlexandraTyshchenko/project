using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Context;
using System.Threading.Tasks;

namespace LinguaDecks.Business.Services
{
	public class CategoryDeletter : ICategoryDeletter
	{
		private readonly ApplicationContext _dbContext;

		public CategoryDeletter(ApplicationContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DeleteCategory(int id)
		{

			var categoryToDelete = await _dbContext.Categories.FindAsync(id);

			if (categoryToDelete != null)
			{
				_dbContext.Categories.Remove(categoryToDelete);
				await _dbContext.SaveChangesAsync();
			}
			else
			{
				throw new NotFoundException("category not found");
			}

		}
	}
}
