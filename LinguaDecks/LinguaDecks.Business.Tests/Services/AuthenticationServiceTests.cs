using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Models;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	[Trait("Category", "Unit")]
	public class AuthenticationServiceTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly IAuthenticationService _sut;

		public AuthenticationServiceTests()
		{
			_contextMock = new Mock<ApplicationContext>();

			_sut = new AuthenticationService(_contextMock.Object);
		}

		[Fact]
		public async Task SignInAsync_WhenValidData_ReturnsUser()
		{
			// Arrange.
			var signInRequest = new SignInRequest
			{
				Email = "teacher@gmail.com",
				Password = "teacher"
			};
			User user = new User
			{
				Id = 1,
				Email = "teacher@gmail.com",
				PasswordHash = "AQAAAAIAAYagAAAAECoNwNS9Eurwp0u+CC4DJ3gTewRugdIcv+Dbc86U1HSXrYZHpXQ4nCmtiDN3T81Uww=="
			};

			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User> { user });

			// Act.
			User result = await _sut.SignInAsync(signInRequest);

			// Assert.
			Assert.Same(user, result);
		}

		[Fact]
		public async Task SignInAsync_WhenUserNotFound_ThrowsNotFoundException()
		{
			// Arrange.
			var signInRequest = new SignInRequest
			{
				Email = "test@gmail.com",
				Password = "123456"
			};

			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(Enumerable.Empty<User>());

			// Act, Assert.
			Func<Task> signInAction = async () => await _sut.SignInAsync(signInRequest);
			await Assert.ThrowsAsync<NotFoundException>(signInAction);
		}

		[Fact]
		public async Task SignInAsync_WhenInvalidPassword_ThrowsBadRequestException()
		{
			// Arrange.
			var signInRequest = new SignInRequest
			{
				Email = "user@gmail.com",
				Password = "dfhdhdf"
			};

			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User>
				{
					new User
					{
						Id = 1,
						Email = "user@gmail.com",
						PasswordHash = "AQAAAAIAAYagAAAAEJ13Tyw1AbQyAk2A0bweycajIqj4etN0P5PjSnhPjulcExQuYY1U0p1nqBUx+ghzBA==" // user
					}
				});

			// Act, Assert.
			Func<Task> signInAction = async () => await _sut.SignInAsync(signInRequest);
			await Assert.ThrowsAsync<BadRequestException>(signInAction);
		}

		[Fact]
		public async Task SignUpAsync_WhenValidData_AddsUser()
		{
			// Arrange.
			List<User> users = new List<User>();

			var signUpRequest = new SignUpRequest
			{
				Email = "testtest@gmail.com",
				Name = "Test test",
				Password = "testtest",
				Role = Roles.Teacher,
				Description = "Test."
			};

			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(users);
			
			// Act.
			User result = await _sut.SignUpAsync(signUpRequest);

			// Assert.
			_contextMock.Verify(context => context.Users.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task SignUpAsync_WhenValidData_SavesChanges()
		{
			// Arrange.
			List<User> users = new List<User>();

			var signUpRequest = new SignUpRequest
			{
				Email = "testtest@gmail.com",
				Name = "Test test",
				Password = "testtest",
				Role = Roles.Teacher,
				Description = "Test."
			};

			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(users);

			// Act.
			User result = await _sut.SignUpAsync(signUpRequest);

			// Assert.
			_contextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task SignUpAsync_WhenValidData_ReturnsUser()
		{
			// Arrange.
			List<User> users = new List<User>();

			var signUpRequest = new SignUpRequest
			{
				Email = "testtest@gmail.com",
				Name = "Test test",
				Password = "testtest",
				Role = Roles.Teacher,
				Description = "Test."
			};
			User expectedUser = new User
			{
				Id = 0,
				Email = "testtest@gmail.com",
				Name = "Test test",
				Role = Roles.Student,
				IconPath = "",
				IsBlocked = false
			};

			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(users);

			// Act.
			User result = await _sut.SignUpAsync(signUpRequest);

			// Assert.
			// can't test equality of passwords
			Assert.Equal(expectedUser.Id, result.Id);
			Assert.Equal(expectedUser.Email, result.Email);
			Assert.Equal(expectedUser.Name, result.Name);
			Assert.Equal(expectedUser.Role, result.Role);
			Assert.Equal(expectedUser.IconPath, result.IconPath);
			Assert.Equal(expectedUser.IsBlocked, result.IsBlocked);
		}

		[Fact]
		public async Task SignUpAsync_WhenEmailExists_ThrowsBadRequestException()
		{
			// Arrange.
			var signUpRequest = new SignUpRequest
			{
				Email = "testuser@gmail.com",
				Name = "Other test",
				Password = "aaaaaaaa",
				Role = Roles.Student,
				Description = "Test desc."
			};

			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User>
				{
					new User
					{
						Id = 1,
						Email = "testuser@gmail.com",
						Name = "Test user",
						Role = Roles.Student,
						PasswordHash = "AQAAAAIAAYagAAAAEJ13Tyw1AbQyAk2A0bweycajIqj4etN0P5PjSnhPjulcExQuYY1U0p1nqBUx+ghzBA==",
						IsBlocked = false,
						IconPath = ""
					}
				});

			// Act, Assert.
			Func<Task> signUpAction = async () => await _sut.SignUpAsync(signUpRequest);
			await Assert.ThrowsAsync<BadRequestException>(signUpAction);
		}
	}
}
