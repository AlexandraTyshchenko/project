using FluentAssertions;
using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
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
	public class ProgressServiceTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly IProgressService _service;

		public ProgressServiceTests()
		{
			_contextMock = new Mock<ApplicationContext>();

			_service = new ProgressService(_contextMock.Object);
		}
		#region GetDeckProgress
		[Fact]
		public async Task GetDeckProgress_WhenDataValid_ReturnsCardProgressList()
		{
			//Arrange
			var deckId = 2;
			var userId = 3;
			List<Card> cards = new List<Card>()
			{
				new(){Id = 0, PrimaryText="p1", SecondaryText = "s1", DeckId = 0},
				new(){Id = 1, PrimaryText="p2", SecondaryText = "s2", DeckId = 0},
				new(){Id = 2, PrimaryText="p3", SecondaryText = "s3", DeckId = 1},
				new(){Id = 3, PrimaryText="p4", SecondaryText = "s4", DeckId = 1},
				new(){Id = 4, PrimaryText="p5", SecondaryText = "s5", DeckId = 1},
				new(){Id = 5, PrimaryText="p6", SecondaryText = "s6", DeckId = 2},
				new(){Id = 6, PrimaryText="p7", SecondaryText = "s7", DeckId = 2},
				new(){Id = 7, PrimaryText="p8", SecondaryText = "s8", DeckId = 2},
				new(){Id = 8, PrimaryText="p9", SecondaryText = "s9", DeckId = 3},
				new(){Id = 9, PrimaryText="p10", SecondaryText = "s10", DeckId = 3},
				new(){Id = 10, PrimaryText="p11", SecondaryText = "s11", DeckId = 4},
			};
			List<CardProgress> progressList = new List<CardProgress>()
			{
				new(){Id = 0, CardId = 5, UserId = 3, IsKnown = true},
				new(){Id = 1, CardId = 6, UserId = 3, IsKnown = false},
				new(){Id = 2, CardId = 7, UserId = 2, IsKnown = true},
				new(){Id = 3, CardId = 7, UserId = 2, IsKnown = false},
				new(){Id = 4, CardId = 4, UserId = 1, IsKnown = true},
				new(){Id = 5, CardId = 3, UserId = 1, IsKnown = false},
				new(){Id = 6, CardId = 2, UserId = 3, IsKnown = true},
				new(){Id = 7, CardId = 1, UserId = 3, IsKnown = false},
			};
			List<CardProgress> expectedResult = new List<CardProgress>()
			{
				new(){Id = 0, CardId = 5, UserId = 3, IsKnown = true},
				new(){Id = 1, CardId = 6, UserId = 3, IsKnown = false},
			};

			_contextMock
				.Setup(context => context.Cards)
				.ReturnsDbSet(cards);
			_contextMock
				.Setup(context => context.CardProgresses)
				.ReturnsDbSet(progressList);

			// Act
			var result = await _service.GetDeckProgress(userId, deckId);

			// Assert
			result.Should().BeEquivalentTo(expectedResult);
		}
		#endregion

		#region GetTotalProgress
		[Fact]
		public async Task GetTotalProgress_DataFilled_ReturnsList()
		{
			//Arrange
			int userId = 3;
			List<Card> cards = new List<Card>()
			{
				new(){Id = 0, PrimaryText="p1", SecondaryText = "s1", DeckId = 0},
				new(){Id = 1, PrimaryText="p2", SecondaryText = "s2", DeckId = 0},
				new(){Id = 2, PrimaryText="p3", SecondaryText = "s3", DeckId = 1},
				new(){Id = 3, PrimaryText="p4", SecondaryText = "s4", DeckId = 1},
				new(){Id = 4, PrimaryText="p5", SecondaryText = "s5", DeckId = 1},
				new(){Id = 5, PrimaryText="p6", SecondaryText = "s6", DeckId = 2},
				new(){Id = 6, PrimaryText="p7", SecondaryText = "s7", DeckId = 2},
				new(){Id = 7, PrimaryText="p8", SecondaryText = "s8", DeckId = 2},
				new(){Id = 8, PrimaryText="p9", SecondaryText = "s9", DeckId = 3},
				new(){Id = 9, PrimaryText="p10", SecondaryText = "s10", DeckId = 3},
				new(){Id = 10, PrimaryText="p11", SecondaryText = "s11", DeckId = 4},
			};
			List<CardProgress> progressList = new List<CardProgress>()
			{
				new(){Id = 0, CardId = 5, Card = cards[5], UserId = 3, IsKnown = true},
				new(){Id = 1, CardId = 6, Card = cards[6], UserId = 3, IsKnown = false},
				new(){Id = 2, CardId = 10, Card = cards[10], UserId = 2, IsKnown = true},
				new(){Id = 3, CardId = 7, Card = cards[7], UserId = 2, IsKnown = false},
				new(){Id = 4, CardId = 4, Card = cards[4], UserId = 1, IsKnown = true},
				new(){Id = 5, CardId = 3, Card = cards[3], UserId = 1, IsKnown = false},
				new(){Id = 6, CardId = 2, Card = cards[2], UserId = 3, IsKnown = true},
				new(){Id = 7, CardId = 1, Card = cards[1], UserId = 3, IsKnown = false},
			};
			Dictionary<int, IEnumerable<CardProgress>> expectedResult = new()
			{
				{progressList[0].Card.DeckId, new List<CardProgress>
					{
						progressList[0],
						progressList[1],
					} 
				}
				,
				{progressList[6].Card.DeckId, new List<CardProgress>
					{
						progressList[6],
					}
				},
				{progressList[7].Card.DeckId, new List<CardProgress>
					{
						progressList[7],
					} 
				}
			};
			_contextMock
				.Setup(context => context.CardProgresses)
				.ReturnsDbSet(progressList);

			//Act
			var result = await _service.GetTotalProgress(userId);

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}
		#endregion

		#region CalculateProgress
		[Fact]
		public	async Task CalculateProgress_WhenDeckExist_ReturnsCalcultedTuple()
		{
			//Arrange
			int deckId = 0;
			List<Card> cards = new List<Card>()
			{
				new(){Id = 0, PrimaryText="p1", SecondaryText = "s1", DeckId = 0},
				new(){Id = 1, PrimaryText="p2", SecondaryText = "s2", DeckId = 0},
				new(){Id = 2, PrimaryText="p3", SecondaryText = "s3", DeckId = 0},
				new(){Id = 3, PrimaryText="p4", SecondaryText = "s4", DeckId = 0},
				new(){Id = 4, PrimaryText="p5", SecondaryText = "s5", DeckId = 0},
				new(){Id = 5, PrimaryText="p6", SecondaryText = "s6", DeckId = 0},
				new(){Id = 6, PrimaryText="p7", SecondaryText = "s7", DeckId = 0},
				new(){Id = 7, PrimaryText="p8", SecondaryText = "s8", DeckId = 0},
				new(){Id = 8, PrimaryText="p9", SecondaryText = "s9", DeckId = 0},
				new(){Id = 9, PrimaryText="p10", SecondaryText = "s10", DeckId = 0},
				new(){Id = 10, PrimaryText="p11", SecondaryText = "s11", DeckId = 0},
			};
			List<CardProgress> progresses = new()
			{
				new(){Id = 0, CardId = 5, Card = cards[5], UserId = 3, IsKnown = true},
				new(){Id = 1, CardId = 6, Card = cards[6], UserId = 3, IsKnown = false},
				new(){Id = 2, CardId = 10, Card = cards[10], UserId = 3, IsKnown = true},
				new(){Id = 3, CardId = 7, Card = cards[7], UserId = 3, IsKnown = false},
				new(){Id = 4, CardId = 4, Card = cards[4], UserId = 3, IsKnown = true},
				new(){Id = 5, CardId = 3, Card = cards[3], UserId = 3, IsKnown = false},
				new(){Id = 6, CardId = 2, Card = cards[2], UserId = 3, IsKnown = true},
				new(){Id = 7, CardId = 1, Card = cards[1], UserId = 3, IsKnown = false},
			};
			List<Deck> decks = new List<Deck>() 
			{
				new()
				{
					Id = 0, Name = "name1", Description = "desc1", Cards = cards, 
					PrimaryLanguage = "pr1", SecondaryLanguage = "sc1",AuthorId = 1,
					IsVisible = true, CategoryId = 1, IconPath = "path1"
				},
				new()
				{
					Id = 2, Name = "name2", Description = "desc2", Cards = new List<Card>(),
					PrimaryLanguage = "pr1", SecondaryLanguage = "sc1",AuthorId = 2,
					IsVisible = true, CategoryId = 2, IconPath = "path2"
				},
			};
			(int positive, int negative, int total) expectedResult = (4, 4, 11);

			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(decks);

			//Act
			var result = await _service.CalculateProgress(deckId, progresses);

			//Assert
			result.Should().Be(expectedResult);

		}
		#endregion
	}
}