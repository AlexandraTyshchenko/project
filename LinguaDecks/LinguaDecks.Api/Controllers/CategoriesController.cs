using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinguaDecks.Api.Models;
using LinguaDecks.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinguaDecks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesProvider _categoriesGetter;
		private readonly IMapper _mapper;
		private readonly ICategoriesCreator _categoriesCreator;
		private readonly ICategoryDeletter _categoriesDeletter;

		public CategoriesController(ICategoriesProvider categoriesGetter,IMapper mapper,ICategoriesCreator categoriesCreator,ICategoryDeletter categoryDeletter)
		{
			_categoriesGetter = categoriesGetter;
			_mapper = mapper;
			_categoriesCreator = categoriesCreator;
			_categoriesDeletter = categoryDeletter;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			var result = await _categoriesGetter.GetCategoryAsync(); 

			var mappedResult = result.Select(category => _mapper.Map<CategoryModel>(category)).ToList();

			return Ok(mappedResult);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> CreateCategory(string categoryName)
		{
				await _categoriesCreator.CreateCategory(categoryName);
				return Ok();
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete]
		public async Task<IActionResult> DeleteCategory(int id)
		{
				await _categoriesDeletter.DeleteCategory(id);
				return Ok();
		}

	}
}
