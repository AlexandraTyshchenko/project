using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	[Trait("Category", "Unit")]
	public class CategoriesProviderTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly ICategoriesProvider _categoriesProvider;

		public CategoriesProviderTests()
		{
			_contextMock = new Mock<ApplicationContext>();
			_categoriesProvider = new CategoriesProvider(_contextMock.Object);
		}

		[Fact]
		public async Task GetCategoryAsync_ReturnsICollection()
		{
			// Arrange
			_contextMock.Setup(context => context.Categories)
						.ReturnsDbSet(new List<Category>());

			// Act
			var result = await _categoriesProvider.GetCategoryAsync();

			// Assert
			Assert.NotNull(result);
			Assert.IsAssignableFrom<ICollection<Category>>(result);
		}
	}
}
