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
	public class RequestsProviderTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly IRequestsProvider _sut;

		public RequestsProviderTests()
		{
			_contextMock = new Mock<ApplicationContext>();
			_sut = new RequestsProvider(_contextMock.Object);
		}

		[Fact]
		public async Task GetTeacherRequests_PendingStatus_ReturnsListOFPendingRequests()
		{
			// Arrange
			var requests = new List<TeacherRequest>()
			{
				new TeacherRequest() { Id = 1, UserId = 1, Status = Statuses.Pending },
				new TeacherRequest() { Id = 2, UserId = 1, Status = Statuses.Pending },
				new TeacherRequest() { Id = 3, UserId = 1, Status = Statuses.Rejected },
				new TeacherRequest() { Id = 4, UserId = 1, Status = Statuses.Pending }
			};
			_contextMock.Setup(context => context.TeacherRequests)
						.ReturnsDbSet(requests.AsQueryable());

			// Act
			var result = await _sut.GetTeacherRequests(1, 3, Statuses.Pending);
			var resultList = result.ToList();

			// Assert
			Assert.All(result, request => Assert.Equal(Statuses.Pending, request.Status));
			Assert.Equal(3, resultList.Count);
		}

		[Fact]
		public async Task GetTeacherRequests_RejectedStatus_ReturnsListOFPendingRequests()
		{
			// Arrange
			var requests = new List<TeacherRequest>()
			{
				new TeacherRequest() { Id = 1, UserId = 1, Status = Statuses.Pending },
				new TeacherRequest() { Id = 2, UserId = 1, Status = Statuses.Rejected },
				new TeacherRequest() { Id = 3, UserId = 1, Status = Statuses.Rejected },
				new TeacherRequest() { Id = 4, UserId = 1, Status = Statuses.Rejected }
			};
			_contextMock.Setup(context => context.TeacherRequests)
						.ReturnsDbSet(requests.AsQueryable());

			// Act
			var result = await _sut.GetTeacherRequests(1, 3, Statuses.Rejected);
			var resultList = result.ToList();

			// Assert
			Assert.All(result, request => Assert.Equal(Statuses.Rejected, request.Status));
			Assert.Equal(3, resultList.Count);
		}

		[Fact]
		public async Task GetTeacherRequests_ApprovedStatus_ReturnsListOFPendingRequests()
		{
			// Arrange
			var requests = new List<TeacherRequest>()
			{
				new TeacherRequest() { Id = 1, UserId = 1, Status = Statuses.Pending },
				new TeacherRequest() { Id = 2, UserId = 1, Status = Statuses.Approved },
				new TeacherRequest() { Id = 3, UserId = 1, Status = Statuses.Approved },
				new TeacherRequest() { Id = 4, UserId = 1, Status = Statuses.Approved }
			};
			_contextMock.Setup(context => context.TeacherRequests)
						.ReturnsDbSet(requests);

			// Act
			var result = await _sut.GetTeacherRequests(1, 3, Statuses.Approved);
			var resultList = result.ToList();

			// Assert
			Assert.All(result, request => Assert.Equal(Statuses.Approved, request.Status));
			Assert.Equal(3, resultList.Count);
		}

		[Fact]
		public async Task GetTeacherRequests_UnExistingPage_ReturnsEmptyList()
		{
			// Arrange
			var requests = new List<TeacherRequest>()
			{
				new TeacherRequest() { Id = 1, UserId = 1, Status = Statuses.Pending },
				new TeacherRequest() { Id = 2, UserId = 1, Status = Statuses.Approved },
				new TeacherRequest() { Id = 3, UserId = 1, Status = Statuses.Approved },
				new TeacherRequest() { Id = 4, UserId = 1, Status = Statuses.Approved }
			};
			_contextMock.Setup(context => context.TeacherRequests)
						.ReturnsDbSet(requests);

			// Act
			var result = await _sut.GetTeacherRequests(2, 3, Statuses.Approved);
			var resultList = result.ToList();

			// Assert
			Assert.Empty(resultList);
		}
	}
}
