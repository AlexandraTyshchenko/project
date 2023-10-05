using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	[Trait("Category", "Unit")]
	public class CategoryCreatorTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly ICategoriesCreator _sut;

		public CategoryCreatorTests()
		{
			_contextMock = new Mock<ApplicationContext>();
			_sut = new CategoriesCreator(_contextMock.Object);
		}

		[Fact]
		public async Task CreateCategory_AddsCategoryToContext()
		{
			// Arrange
			var categories = new List<Category>();
			_contextMock
				.Setup(context => context.Categories)
				.ReturnsDbSet(categories);
			_contextMock
				.Setup(db => db.Categories.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
				.Callback((Category Category, CancellationToken token) => categories.Add(Category))
				.Returns(new ValueTask<EntityEntry<Category>>(Task.FromResult((EntityEntry<Category>)null)));

			// Act
			await _sut.CreateCategory("some category");

			// Assert
			Assert.Single(_contextMock.Object.Categories);
		}

		[Fact]
		public async Task CreateCategory_CategoryExists_ThrowsException()
		{
			// Arrange
			var categories = new List<Category>() { new Category() { Name = "some category" } };
			_contextMock
				.Setup(context => context.Categories)
				.ReturnsDbSet(categories);

			// Act and Assert
			await Assert.ThrowsAsync<BadRequestException>(() => _sut.CreateCategory("some category"));
		}
	}
}
