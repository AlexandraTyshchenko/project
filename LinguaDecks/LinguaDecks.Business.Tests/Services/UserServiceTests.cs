using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	[Trait("Category", "Unit")]
	public class UserServiceTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly IUserService _service;

		public UserServiceTests()
		{
			_contextMock = new Mock<ApplicationContext>();

			_service = new UserService(_contextMock.Object);
		}
		#region GetUser
		[Fact]
		public async Task GetUser_WhenUserFound_ReturnsUserModel()
		{
			//Arrange
			var userId = 3;
			User user = new User()
			{
				Id = 3,
				Email = "mail@gmail.com",
				Name = "Test",
				Role = Roles.Admin,
				PasswordHash = "HSDW++SDA12=",
				IsBlocked = false,
				IconPath = "path",
			};

			_contextMock
				.Setup(context => context.Users.FindAsync(userId))
				.Returns(new ValueTask<User>(new List<User>() { user }.Find(b => b.Id == userId)));

			// Act
			var returnedUser = await _service.GetUser(userId);

			// Assert
			Assert.Equal(user, returnedUser);
		}

		[Fact]
		public async Task GetUserTest_WhenUserNotFound_ThrowsException()
		{
			//Arrange
			int userId = 5;
			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(Enumerable.Empty<User>());

			// Act
			Func<Task> getUserAction = async () => await _service.GetUser(userId);

			// Assert
			await Assert.ThrowsAsync<NotFoundException>(getUserAction);
		}
		#endregion
	}
}