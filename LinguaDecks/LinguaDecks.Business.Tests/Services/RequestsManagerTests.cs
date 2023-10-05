using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	[Trait("Category", "Unit")]
	public class RequestsManagerTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly IRequestManager _sut;
		private readonly List<TeacherRequest> _requests;
		private readonly List<User> _users;

		public RequestsManagerTests()
		{
			_contextMock = new Mock<ApplicationContext>();
			_sut = new RequestManager(_contextMock.Object);
			_requests = new List<TeacherRequest>()
			{
				new TeacherRequest() { Id = 1, UserId = 1, Status = Statuses.Pending },
				new TeacherRequest() { Id = 2, UserId = 2, Status = Statuses.Pending },
				new TeacherRequest() { Id = 3, UserId = 3, Status = Statuses.Pending },
				new TeacherRequest() { Id = 4, UserId = 4, Status = Statuses.Pending },
			};
			_users = new List<User>()
			{
				new User() { Id = 1, Role = Roles.Student },
				new User() { Id = 2, Role = Roles.Student },
				new User() { Id = 3, Role = Roles.Student },
				new User() { Id = 4, Role = Roles.Student },
			};
		}

		[Fact]
		public async Task AnswerRequest_RequestIsApproved_RoleChangesToTeacher()
		{
			// Arrange
			_contextMock.Setup(context => context.TeacherRequests).ReturnsDbSet(_requests);
			_contextMock.Setup(context => context.Users).ReturnsDbSet(_users);

			// Act
			await _sut.AnswerRequest(2, true);

			// Assert
			var updatedRequest = _requests.FirstOrDefault(r => r.UserId == 2);
			var updatedUser = _users.FirstOrDefault(u => u.Id == 2);
			Assert.Equal(Statuses.Approved, updatedRequest.Status);
			Assert.Equal(Roles.Teacher, updatedUser.Role);
		}

		[Fact]
		public async Task AnswerRequest_RequestIsRejected_RoleChangesToTeacher()
		{
			// Arrange
			_contextMock.Setup(context => context.TeacherRequests).ReturnsDbSet(_requests);
			_contextMock.Setup(context => context.Users).ReturnsDbSet(_users);

			// Act
			await _sut.AnswerRequest(2, false);

			// Assert
			var updatedRequest = _requests.FirstOrDefault(r => r.UserId == 2);
			var updatedUser = _users.FirstOrDefault(u => u.Id == 2);
			Assert.Equal(Statuses.Rejected, updatedRequest.Status);
			Assert.Equal(Roles.Student, updatedUser.Role);
		}

		[Fact]
		public async Task AnswerRequest_UnexistingUser_ThrowsException()
		{
			// Arrange
			_contextMock.Setup(context => context.TeacherRequests).ReturnsDbSet(_requests);
			_contextMock.Setup(context => context.Users).ReturnsDbSet(_users);

			// Act and Assert
			await Assert.ThrowsAsync<NotFoundException>(() => _sut.AnswerRequest(5, false));
		}
	}
}
