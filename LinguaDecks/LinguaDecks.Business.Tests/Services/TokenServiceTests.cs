using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Models;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	[Trait("Category", "Unit")]
	public class TokenServiceTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly Mock<IConfiguration> _configurationMock;
		private readonly ITokenService _sut;

		public TokenServiceTests()
		{
			_contextMock = new Mock<ApplicationContext>();
			_configurationMock = new Mock<IConfiguration>();
			_sut = new TokenService(_contextMock.Object, _configurationMock.Object);
		}

		[Fact]
		public async Task CreateTokenAsync_WhenRefreshTokenExists_ReturnsTokenResponse()
		{
			// Arrange
			User user = new User
			{
				Id = 1,
				Email = "testuser2@gmail.com",
				Name = "Test 3 test",
				Role = Roles.Student,
				PasswordHash = "AQAAAAIAAYagAAAAEJ13Tyw1AbQyAk2A0bweycajIqj4etN0P5PjSnhPjulcExQuYY1U0p1nqBUx+ghzBA==" // user
			};
			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User> { user });
			_contextMock
				.Setup(context => context.RefreshTokens)
				.ReturnsDbSet(new List<RefreshToken>
				{
					new RefreshToken
					{
						Id = 1,
						UserId = 1,
						Value = Guid.NewGuid().ToString(),
						ExpirationDate = DateTime.Now.AddDays(30)
					}
				});
			_configurationMock
				.Setup(c => c["JwtSettings:Issuer"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Audience"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Key"])
				.Returns("SomeLongJwtSecurityKey");

			// Act
			TokenResponse result = await _sut.CreateTokenAsync(user);

			// Assert
			Assert.NotEmpty(result.Token);
			Assert.IsType<string>(result.RefreshToken.Value);
			Assert.Equal(DateTime.Now.AddDays(30).Year, result.RefreshToken.ExpirationDate.Year);
			Assert.Equal(DateTime.Now.AddDays(30).Month, result.RefreshToken.ExpirationDate.Month);
			Assert.Equal(DateTime.Now.AddDays(30).Day, result.RefreshToken.ExpirationDate.Day);
			Assert.Equal(DateTime.Now.AddDays(30).Hour, result.RefreshToken.ExpirationDate.Hour);
			Assert.Equal(DateTime.Now.AddDays(30).Minute, result.RefreshToken.ExpirationDate.Minute);
		}

		[Fact]
		public async Task CreateTokenAsync_WhenRefreshTokenDoesntExistAndValidUser_AddsRefreshToken()
		{
			// Arrange
			List<RefreshToken> items = new List<RefreshToken>();
			User user = new User
			{
				Id = 1,
				Email = "othertest@gmail.com",
				Name = "Other test",
				Role = Roles.Teacher,
				PasswordHash = "AQAAAAIAAYagAAAAEJ13Tyw1AbQyAk2A0bweycajIqj4etN0P5PjSnhPjulcExQuYY1U0p1nqBUx+ghzBA==" // user
			};
			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User> { user });
			_contextMock
				.Setup(context => context.Users.FindAsync(user.Id))
				.ReturnsAsync(user);
			_contextMock
				.Setup(context => context.RefreshTokens)
				.ReturnsDbSet(Enumerable.Empty<RefreshToken>());
			_configurationMock
				.Setup(c => c["JwtSettings:Issuer"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Audience"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Key"])
				.Returns("SomeLongJwtSecurityKey");

			// Act
			await _sut.CreateTokenAsync(user);

			// Assert
			_contextMock.Verify(context => context.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task CreateTokenAsync_WhenRefreshTokenDoesntExistAndValidUser_SavesChanges()
		{
			// Arrange
			List<RefreshToken> items = new List<RefreshToken>();
			User user = new User
			{
				Id = 1,
				Email = "othertest@gmail.com",
				Name = "Other test",
				Role = Roles.Teacher,
				PasswordHash = "AQAAAAIAAYagAAAAEJ13Tyw1AbQyAk2A0bweycajIqj4etN0P5PjSnhPjulcExQuYY1U0p1nqBUx+ghzBA==" // user
			};
			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User> { user });
			_contextMock
				.Setup(context => context.Users.FindAsync(user.Id))
				.ReturnsAsync(user);
			_contextMock
				.Setup(context => context.RefreshTokens)
				.ReturnsDbSet(Enumerable.Empty<RefreshToken>());
			_configurationMock
				.Setup(c => c["JwtSettings:Issuer"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Audience"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Key"])
				.Returns("SomeLongJwtSecurityKey");

			// Act
			await _sut.CreateTokenAsync(user);

			// Assert
			_contextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task RefreshTokenAsync_WhenValidToken_ReturnsTokenResponse()
		{
			// Arrange
			User user = new User
			{
				Id = 1022,
				Email = "sometest@gmail.com",
				Name = "Testtesttest",
				Role = Roles.Student,
				PasswordHash = "AQAAAAIAAYagAAAAEDlLzd1mfu2rpjD90hX8pMdYJHutN6QOcAl5XY2rKbVravnabCe3/FUMCvmca4apGA=="
			};
			RefreshToken refreshToken = new RefreshToken
			{
				Id = 1016,
				UserId = 1022,
				Value = Guid.NewGuid().ToString(),
				ExpirationDate = DateTime.Now.AddDays(30)
			};
			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User> { user });
			_contextMock
				.Setup(context => context.RefreshTokens)
				.ReturnsDbSet(new List<RefreshToken> { refreshToken });
			_contextMock
				.Setup(context => context.Users.FindAsync(user.Id))
				.ReturnsAsync(user);
			_contextMock
				.Setup(context => context.RefreshTokens.FindAsync(refreshToken.Id))
				.ReturnsAsync(refreshToken);
			_configurationMock
				.Setup(c => c["JwtSettings:Issuer"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Audience"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Key"])
				.Returns("SomeLongJwtSecurityKey");

			// Act
			TokenResponse result = await _sut.RefreshTokenAsync("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIxMDIyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IlRlc3R0ZXN0dGVzdCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InNvbWV0ZXN0QGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlN0dWRlbnQiLCJyZWZyZXNoVG9rZW5JZCI6IjEwMTYiLCJleHAiOjE2OTUxMzIwOTYsImlzcyI6Ikxpbmd1YURlY2tzIiwiYXVkIjoiTGluZ3VhRGVja3MifQ.TLcLuJXphgiKFE4jCyCyioVE8TuyPbsPgn5Bmmwt1ko");

			// Assert
			Assert.NotEmpty(result.Token);
			Assert.IsType<string>(result.RefreshToken.Value);
			Assert.Equal(DateTime.Now.AddDays(30).Year, result.RefreshToken.ExpirationDate.Year);
			Assert.Equal(DateTime.Now.AddDays(30).Month, result.RefreshToken.ExpirationDate.Month);
			Assert.Equal(DateTime.Now.AddDays(30).Day, result.RefreshToken.ExpirationDate.Day);
			Assert.Equal(DateTime.Now.AddDays(30).Hour, result.RefreshToken.ExpirationDate.Hour);
			Assert.Equal(DateTime.Now.AddDays(30).Minute, result.RefreshToken.ExpirationDate.Minute);
		}

		[Fact]
		public async Task RefreshTokenAsync_WhenValidToken_RemovesRefreshToken()
		{
			// Arrange
			User user = new User
			{
				Id = 1022,
				Email = "sometest@gmail.com",
				Name = "Testtesttest",
				Role = Roles.Student,
				PasswordHash = "AQAAAAIAAYagAAAAEDlLzd1mfu2rpjD90hX8pMdYJHutN6QOcAl5XY2rKbVravnabCe3/FUMCvmca4apGA=="
			};
			RefreshToken refreshToken = new RefreshToken
			{
				Id = 1016,
				UserId = 1022
			};
			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User> { user });
			_contextMock
				.Setup(context => context.RefreshTokens)
				.ReturnsDbSet(new List<RefreshToken> { refreshToken });
			_contextMock
				.Setup(context => context.Users.FindAsync(user.Id))
				.ReturnsAsync(user);
			_contextMock
				.Setup(context => context.RefreshTokens.FindAsync(refreshToken.Id))
				.ReturnsAsync(refreshToken);
			_configurationMock
				.Setup(c => c["JwtSettings:Issuer"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Audience"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Key"])
				.Returns("SomeLongJwtSecurityKey");

			// Act
			await _sut.RefreshTokenAsync("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIxMDIyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IlRlc3R0ZXN0dGVzdCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InNvbWV0ZXN0QGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlN0dWRlbnQiLCJyZWZyZXNoVG9rZW5JZCI6IjEwMTYiLCJleHAiOjE2OTUxMzIwOTYsImlzcyI6Ikxpbmd1YURlY2tzIiwiYXVkIjoiTGluZ3VhRGVja3MifQ.TLcLuJXphgiKFE4jCyCyioVE8TuyPbsPgn5Bmmwt1ko");

			// Assert
			_contextMock.Verify(context => context.RefreshTokens.Remove(refreshToken), Times.Once);
		}

		[Fact]
		public async Task RefreshTokenAsync_WhenValidToken_SavesChanges()
		{
			// Arrange
			User user = new User
			{
				Id = 1022,
				Email = "sometest@gmail.com",
				Name = "Testtesttest",
				Role = Roles.Student,
				PasswordHash = "AQAAAAIAAYagAAAAEDlLzd1mfu2rpjD90hX8pMdYJHutN6QOcAl5XY2rKbVravnabCe3/FUMCvmca4apGA=="
			};
			RefreshToken refreshToken = new RefreshToken
			{
				Id = 1016,
				UserId = 1022
			};
			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User> { user });
			_contextMock
				.Setup(context => context.RefreshTokens)
				.ReturnsDbSet(new List<RefreshToken> { refreshToken });
			_contextMock
				.Setup(context => context.Users.FindAsync(user.Id))
				.ReturnsAsync(user);
			_contextMock
				.Setup(context => context.RefreshTokens.FindAsync(refreshToken.Id))
				.ReturnsAsync(refreshToken);
			_configurationMock
				.Setup(c => c["JwtSettings:Issuer"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Audience"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Key"])
				.Returns("SomeLongJwtSecurityKey");

			// Act
			await _sut.RefreshTokenAsync("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIxMDIyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IlRlc3R0ZXN0dGVzdCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InNvbWV0ZXN0QGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlN0dWRlbnQiLCJyZWZyZXNoVG9rZW5JZCI6IjEwMTYiLCJleHAiOjE2OTUxMzIwOTYsImlzcyI6Ikxpbmd1YURlY2tzIiwiYXVkIjoiTGluZ3VhRGVja3MifQ.TLcLuJXphgiKFE4jCyCyioVE8TuyPbsPgn5Bmmwt1ko");

			// Assert
			_contextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
		}

		[Fact]
		public async Task RefreshTokenAsync_WhenInvalidToken_ThrowsBadRequestException()
		{
			// Arrange
			User user = new User
			{
				Id = 1022,
				Email = "sometest@gmail.com",
				Name = "Testtesttest",
				Role = Roles.Student,
				PasswordHash = "AQAAAAIAAYagAAAAEDlLzd1mfu2rpjD90hX8pMdYJHutN6QOcAl5XY2rKbVravnabCe3/FUMCvmca4apGA=="
			};
			RefreshToken refreshToken = new RefreshToken
			{
				Id = 1016,
				UserId = 1022
			};
			_contextMock
				.Setup(context => context.Users)
				.ReturnsDbSet(new List<User> { user });
			_contextMock
				.Setup(context => context.RefreshTokens)
				.ReturnsDbSet(new List<RefreshToken> { refreshToken });
			_contextMock
				.Setup(context => context.Users.FindAsync(user.Id))
				.ReturnsAsync(user);
			_contextMock
				.Setup(context => context.RefreshTokens.FindAsync(refreshToken.Id))
				.ReturnsAsync(refreshToken);
			_configurationMock
				.Setup(c => c["JwtSettings:Issuer"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Audience"])
				.Returns("LinguaDecks");
			_configurationMock
				.Setup(c => c["JwtSettings:Key"])
				.Returns("SomeLongJwtSecurityKey");

			// Act, Assert
			await Assert.ThrowsAsync<BadRequestException>(async () => await _sut.RefreshTokenAsync("yJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIxMDIyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IlRlc3R0ZXN0dGVzdCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InNvbWV0ZXN0QGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlN0dWRlbnQiLCJyZWZyZXNoVG9rZW5JZCI6IjEwMTYiLCJleHAiOjE2OTUxMzIwOTYsImlzcyI6Ikxpbmd1YURlY2tzIiwiYXVkIjoiTGluZ3VhRGVja3MifQ.TLcLuJXphgiKFE4jCyCyioVE8TuyPbsPgn5Bmmwt1ko"));
		}
	}
}
