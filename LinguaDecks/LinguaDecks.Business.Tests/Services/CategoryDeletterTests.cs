using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	public class CategoryDeletterTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly ICategoryDeletter _sut;

		public CategoryDeletterTests()
		{
			_contextMock = new Mock<ApplicationContext>();
			_sut = new CategoryDeletter(_contextMock.Object);
		}

		[Fact]
		public async Task DeleteCategory_ValidId_DeletesCategory()
		{
			// Arrange
			var category = new Category() { Id = 1 };
			var categories = new List<Category>() { new Category { Id = 1 } };
			_contextMock
				.Setup(context => context.Categories)
				.ReturnsDbSet(categories);
			_contextMock
				.Setup(context => context.Categories.FindAsync(1))
				.ReturnsAsync(category);

			// Act
			await _sut.DeleteCategory(1);

			// Assert
			_contextMock.Verify(context => context.Categories.Remove(category), Times.Once);
		}

		[Fact]
		public async Task DeleteCategory_UnexistingId_DeletesCategory()
		{
			// Arrange
			var categories = new List<Category>() { new Category { Id = 2 } };
			_contextMock
				.Setup(context => context.Categories)
				.ReturnsDbSet(categories);

			// Act and Assert
			await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteCategory(1));
		}
	}
}
