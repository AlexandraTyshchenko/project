using System.Collections.Generic;
using System.Threading.Tasks;
using LinguaDecks.DataAccess.Entities;

namespace LinguaDecks.Business.Interfaces
{
	public interface ICategoriesProvider
	{
		Task<ICollection<Category>> GetCategoryAsync();
	
	}
}
