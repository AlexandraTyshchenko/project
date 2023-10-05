using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	[Trait("Category", "Unit")]
	public class TeacherRequestCreatorTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly ITeacherRequestCreator _sut;

		public TeacherRequestCreatorTests()
		{
			_contextMock = new Mock<ApplicationContext>();

			_sut = new TeacherRequestCreator(_contextMock.Object);
		}

		[Fact]
		public async Task CreateAsync_WhenValidRequest_AddsTeacherRequest()
		{
			// Arrange
			List<TeacherRequest> items = new List<TeacherRequest>();

			_contextMock
				.Setup(context => context.TeacherRequests)
				.ReturnsDbSet(items);

			// Act
			await _sut.CreateAsync(1, "Test description.");

			// Assert
			_contextMock.Verify(context => context.TeacherRequests.AddAsync(It.IsAny<TeacherRequest>(), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task CreateAsync_WhenValidRequest_SavesChanges()
		{
			// Arrange
			List<TeacherRequest> items = new List<TeacherRequest>();

			_contextMock
				.Setup(context => context.TeacherRequests)
				.ReturnsDbSet(items);

			// Act
			await _sut.CreateAsync(7, "Testtestetes.");

			// Assert
			_contextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task CreateAsync_WhenValidRequest_ReturnsTeacherRequest()
		{
			// Arrange
			List<TeacherRequest> items = new List<TeacherRequest>();
			TeacherRequest expected = new TeacherRequest
			{
				Id = 0,
				UserId = 1000,
				Description = "sdsgfhsdhash",
				Status = Statuses.Pending
			};

			_contextMock
				.Setup(context => context.TeacherRequests)
				.ReturnsDbSet(items);

			// Act
			TeacherRequest result = await _sut.CreateAsync(1000, "sdsgfhsdhash");

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task CreateAsync_WhenRequestWithUserIdExists_ThrowsBadRequestException()
		{
			// Arrange
			List<TeacherRequest> items = new List<TeacherRequest>
			{
				new TeacherRequest
				{
					Id = 5,
					UserId = 8
				}
			};

			_contextMock
				.Setup(context => context.TeacherRequests)
				.ReturnsDbSet(items);

			// Act, Assert
			await Assert.ThrowsAsync<BadRequestException>(async () => await _sut.CreateAsync(8, "Invalid request"));
		}
	}
}
