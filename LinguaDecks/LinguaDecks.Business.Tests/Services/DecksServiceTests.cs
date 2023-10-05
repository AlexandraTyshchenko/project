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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	[Trait("Category", "Unit")]
	public class DecksServiceTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly Mock<IImageService> _imageServiceMock;
		private readonly IDecksService _service;

		public DecksServiceTests()
		{
			_contextMock = new Mock<ApplicationContext>();
			_imageServiceMock = new Mock<IImageService>();

			_service = new DecksService(_contextMock.Object, _imageServiceMock.Object);
		}
		#region GetDeckById
		[Fact]
		public async Task GetDeckByIdTest_WhenDeckFound_ReturnsDeckModel()
		{
			//Arrange
			var deckId = 3;
			User author = new User()
			{
				Id = 3,
				Email = "mail@gmail.com",
				Name = "Test",
				Role = Roles.Teacher,
				PasswordHash = "HSDW++SDA12=",
				IsBlocked = false,
				IconPath = "path",
			};
			List<Card> cards = new List<Card>
			{
				new Card { Id = 1, DeckId = deckId, PrimaryText = "primary text 1", SecondaryText = "secondary text 1" },
				new Card { Id = 5, DeckId = deckId, PrimaryText = "primary text 2", SecondaryText = "secondary text 2" },
				new Card { Id = 3, DeckId = deckId, PrimaryText = "primary text 3", SecondaryText = "secondary text 3" },
			};
			Deck deck = new Deck
			{
				Id = 3,
				Name = "Test deck",
				Description = "description",
				Cards = cards,
				PrimaryLanguage = "en",
				SecondaryLanguage = "fr",
				AuthorId = author.Id,
				Author = author,
				IsVisible = true,
				CategoryId = 1,
				IconPath = "path"
			};

			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(new List<Deck> { deck });

			_contextMock.Setup(context => context.Decks
				.FindAsync(deckId))
				.ReturnsAsync(deck);

			// Act
			var returnedDeck = await _service.GetDeckById(deckId);

			// Assert
			Assert.Equal(deck, returnedDeck);
		}

		[Fact]
		public async Task GetDeckByIdTest_WhenDeckNotFound_ThrowsException()
		{
			//Arrange
			int deckId = 5;

			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(Enumerable.Empty<Deck>());

			// Act
			Func<Task> returnedDeck = async () => await _service.GetDeckById(deckId);

			// Assert
			await Assert.ThrowsAsync<InvalidOperationException>(returnedDeck);
		}
		#endregion

		#region GetDecksRatings
		[Fact]
		public async Task GetDecksRatingsTest_WhenDeckFound_ReturnsRatingList()
		{
			//Arrange
			int deckId = 1;
			var ratingList = new List<Rating> {
				new() { Id = 1, UserId = 1, DeckId = 1, Value = 5 },
				new() { Id = 2, UserId = 2, DeckId = 1, Value = 4 },
				new() { Id = 3, UserId = 3, DeckId = 2, Value = 3 }
			};
			var expected = ratingList.Where(rating => rating.DeckId == deckId);

			_contextMock
				.Setup(context => context.Ratings)
				.ReturnsDbSet(ratingList);

			//Act
			var result = await _service.GetDecksRatings(deckId);

			// Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public async Task GetDecksRatingsTest_WhenDeckNotFound_ReturnsEmptyList()
		{
			//Arrange
			int deckId = 0;
			var ratingList = new List<Rating> {
				new() { Id = 1, UserId = 1, DeckId = 1, Value = 5 },
				new() { Id = 2, UserId = 2, DeckId = 1, Value = 4 },
				new() { Id = 3, UserId = 3, DeckId = 2, Value = 3 }
			};
			var expected = ratingList.Where(rating => rating.DeckId == deckId);

			_contextMock
				.Setup(context => context.Ratings)
				.ReturnsDbSet(ratingList);

			//Act
			var result = await _service.GetDecksRatings(deckId);

			// Assert
			Assert.Empty(result);
		}
		#endregion
		
		#region GetDecksComments
		[Fact]
		public async Task GetDecksCommentsTast_WhenDeckFound_ReturnsCommentList()
		{
			//Arrange
			int deckId = 0;
			User author = new User()
			{
				Id = 3,
				Email = "mail@gmail.com",
				Name = "Test",
				Role = Roles.Teacher,
				PasswordHash = "HSDW++SDA12=",
				IsBlocked = false,
				IconPath = "path",
			};
			List<Comment> comments = new List<Comment>
			{
				new(){Id = 0, UserId = 3, DeckId = 0, Text = "text", Date = DateTime.Now, User = author},
				new(){Id = 1, UserId = 3, DeckId = 1, Text = "text2", Date = DateTime.Now.AddDays(5), User = author},
				new(){Id = 1, UserId = 3, DeckId = 0, Text = "text2", Date = DateTime.Now.AddDays(7), User = author},
			};
			List<Comment> expectedComments = comments.Where(x => x.DeckId == deckId).ToList();

			_contextMock
				.Setup(context => context.Comments)
				.ReturnsDbSet(expectedComments);

			//Act
			var result = await _service.GetDecksComments(deckId);

			//Assert
			Assert.Equal(expectedComments, result);
		}

		[Fact]
		public async Task GetDecksCommentsTast_WhenDeckNotFound_ReturnsEmptyList()
		{
			//Arange
			int deckId = 0;

			_contextMock.Setup(context => context.Comments).ReturnsDbSet(new List<Comment>());

			//Act
			var result = await _service.GetDecksComments(deckId);

			// Assert
			Assert.Empty(result);
		}
		#endregion

		#region GetDeckCards
		[Fact]
		public async Task GetDeckCardsTest_WhenDeckFound_ReturnsCardList()
		{
			//Arrange
			int deckId = 0;
			List<Card> cards = new List<Card>
			{
				new Card { Id = 1, DeckId = 0, PrimaryText = "primary text 1", SecondaryText = "secondary text 1" },
				new Card { Id = 5, DeckId = 1, PrimaryText = "primary text 2", SecondaryText = "secondary text 2" },
				new Card { Id = 3, DeckId = 0, PrimaryText = "primary text 3", SecondaryText = "secondary text 3" },
			};
			List<Card> expectedList = cards.Where(c=>c.DeckId == deckId).ToList();

			_contextMock.Setup(context => context.Cards).ReturnsDbSet(cards);

			//Act
			var result = await _service.GetDeckCards(deckId);

			//Assert
			Assert.Equal(expectedList, result);
		}

		[Fact]
		public async Task GetDeckCardsTest_WhenDeckNotFound_ReturnsEmptyList()
		{
			//Arrange
			int deckId = 5;
			List<Card> cards = new List<Card>
			{
				new Card { Id = 1, DeckId = 0, PrimaryText = "primary text 1", SecondaryText = "secondary text 1" },
				new Card { Id = 5, DeckId = 1, PrimaryText = "primary text 2", SecondaryText = "secondary text 2" },
				new Card { Id = 3, DeckId = 0, PrimaryText = "primary text 3", SecondaryText = "secondary text 3" },
			};
			List<Card> expectedList = cards.Where(c => c.DeckId == deckId).ToList();

			_contextMock.Setup(context => context.Cards).ReturnsDbSet(cards);

			//Act
			var result = await _service.GetDeckCards(deckId);

			//Assert
			Assert.Equal(expectedList, result);
		}
		#endregion

		#region CalculateDeckRating
		[Fact]
		public async Task CalculateDeckRatingTest_CheckCorrectCalculation()
		{
			//Arange
			(float Rating, int Votes) expectedResult = (3.5f, 4);
			List<Rating> ratingList = new List<Rating>()
			{
				new(){Id = 1, UserId = 1, DeckId = 1, Value = 3},
				new(){Id = 2, UserId = 2, DeckId = 1, Value = 4},
				new(){Id = 3, UserId = 3, DeckId = 1, Value = 5},
				new(){Id = 4, UserId = 4, DeckId = 1, Value = 2},
			};

			//Act
			var result = _service.CalculateDeckRating(ratingList);

			//Assert
			Assert.Equal(expectedResult, result);
		}
		#endregion

		#region SaveProgress
		[Fact]
		public async Task SaveProgressTest_WhenCardExist_UpdateValue()
		{
			//Arrange
			List<CardProgress> progresses = new List<CardProgress>()
			{
				new(){Id = 0, CardId = 1, UserId = 1, IsKnown = true},
				new(){Id = 1, CardId = 2, UserId = 1, IsKnown = false},
				new(){Id = 2, CardId = 3, UserId = 1, IsKnown = false},
				new(){Id = 3, CardId = 4, UserId = 1, IsKnown = true},
			};
			List<CardProgress> receivingProgresses = new List<CardProgress>()
			{
				new(){Id = 0, CardId = 1, UserId = 1, IsKnown = true},
				new(){Id = 1, CardId = 2, UserId = 1, IsKnown = false},
				new(){Id = 2, CardId = 3, UserId = 1, IsKnown = true},
			}; 
			List<CardProgress> expectedResult = new List<CardProgress>()
			{
				new(){Id = 0, CardId = 1, UserId = 1, IsKnown = true},
				new(){Id = 1, CardId = 2, UserId = 1, IsKnown = false},
				new(){Id = 2, CardId = 3, UserId = 1, IsKnown = true},
				new(){Id = 3, CardId = 4, UserId = 1, IsKnown = true},
			};

			_contextMock
				.Setup(context => context.CardProgresses)
				.ReturnsDbSet(progresses);

			//Act
			await _service.SaveProgress(receivingProgresses);
			List<CardProgress> result = await _contextMock.Object.CardProgresses.ToListAsync();

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		[Fact]
		public async Task SaveProgressTest_WhenCardDontExist_AddNewCards()
		{
			// Arrange
			List<CardProgress> existingCards = new List<CardProgress>
			{
				new(){Id = 0, CardId = 1, UserId = 1, IsKnown = true},
				new(){Id = 1, CardId = 2, UserId = 1, IsKnown = false},
				new(){Id = 2, CardId = 3, UserId = 1, IsKnown = false},
			};
			List<CardProgress> newCards = new List<CardProgress>
			{
				new(){Id = 3, CardId = 4, UserId = 1, IsKnown = true},
				new(){Id = 4, CardId = 5, UserId = 1, IsKnown = false},
				new(){Id = 5, CardId = 6, UserId = 1, IsKnown = true},
			};

			_contextMock
				.Setup(db => db.CardProgresses)
				.ReturnsDbSet(existingCards);

			_contextMock
				.Setup(db => db.CardProgresses.AddAsync(It.IsAny<CardProgress>(), It.IsAny<CancellationToken>()))
				.Callback((CardProgress card, CancellationToken token) => existingCards.Add(card))
				.Returns(new ValueTask<EntityEntry<CardProgress>>(Task.FromResult((EntityEntry<CardProgress>)null)));

			// Act
			await _service.SaveProgress(newCards);

			// Assert
			existingCards.Should().ContainSingle(card => card.CardId == 4 && card.UserId == 1 && card.IsKnown);
			existingCards.Should().ContainSingle(card => card.CardId == 5 && card.UserId == 1 && !card.IsKnown);
			existingCards.Should().ContainSingle(card => card.CardId == 6 && card.UserId == 1 && card.IsKnown);
		}

		#endregion

		#region GetDeckIcons
		[Fact]
		public async Task GetDeckIconsTest_ReturnsCorrectIcons()
		{
			// Arrange
			var deckIds = new List<int> { 1, 2, 3 };
			var decks = new List<Deck>
			{
				new Deck { Id = 1, IconPath = "path1" },
				new Deck { Id = 2, IconPath = "path2" },
				new Deck { Id = 3, IconPath = "path3" },
				new Deck { Id = 4, IconPath = "path4" }
			};
			var expectedResult = new List<(int deckId, string iconPath)>
			{
				(1, "path1"),
				(2, "path2"),
				(3, "path3")
			};

			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(decks);

			// Act
			var result = await _service.GetDeckIcons(deckIds);

			// Assert
			result.Should().BeEquivalentTo(expectedResult);
		}
		#endregion

		#region CreateDeck
		[Fact]
		public async Task CreateDeck_AddsDeck()
		{
			// Arrange
			List<Deck> items = new List<Deck>();
			DeckCreateRequest request = new DeckCreateRequest
			{
				Name = "Test deck",
				Description = "",
				PrimaryLanguage = "English",
				SecondaryLanguage = "Ukrainian",
				AuthorId = 1,
				CategoryId = 1
			};

			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(items);

			// Act
			await _service.CreateDeck(request);

			// Assert
			_contextMock.Verify(context => context.Decks.AddAsync(It.IsAny<Deck>(), It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task CreateDeck_SavesChanges()
		{
			// Arrange
			List<Deck> items = new List<Deck>();
			DeckCreateRequest request = new DeckCreateRequest
			{
				Name = "aaaaaaaaaaaa",
				Description = "aaaaaaaaaaaa",
				PrimaryLanguage = "German",
				SecondaryLanguage = "German",
				AuthorId = 567,
				CategoryId = 4
			};

			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(items);

			// Act
			await _service.CreateDeck(request);

			// Assert
			_contextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task CreateDeck_ReturnsDeck()
		{
			// Arrange
			List<Deck> items = new List<Deck>();
			DeckCreateRequest request = new DeckCreateRequest
			{
				Name = "Namename",
				Description = ".",
				PrimaryLanguage = "Other",
				SecondaryLanguage = "Other",
				AuthorId = 76,
				CategoryId = 34
			};
			Deck expected = new Deck
			{
				Id = 0,
				Name = "Namename",
				Description = ".",
				PrimaryLanguage = "Other",
				SecondaryLanguage = "Other",
				AuthorId = 76,
				CategoryId = 34,
				IsVisible = true,
				IconPath = string.Empty
			};
			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(items);

			// Act
			Deck result = await _service.CreateDeck(request);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}
		#endregion

		#region UpdateDeck
		[Fact]
		public async Task UpdateDeck_WhenDeckExists_UpdatesDeck()
		{
			// Arrange
			Deck deck = new Deck
			{
				Id = 56,
				Name = "Test deck",
				Description = "",
				PrimaryLanguage = "French",
				SecondaryLanguage = "Italian",
				AuthorId = 34,
				IsVisible = true,
				CategoryId = 12,
				IconPath = ""
			};
			DeckUpdateRequest request = new DeckUpdateRequest
			{
				Name = "Other test deck",
				Description = "Somedesc",
				PrimaryLanguage = "Italian",
				SecondaryLanguage = "French",
				CategoryId = 22
			};

			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(new List<Deck> { deck });
			_contextMock
				.Setup(context => context.Decks.FindAsync(deck.Id))
				.ReturnsAsync(deck);

			// Act
			await _service.UpdateDeck(56, request);

			// Assert
			Assert.Equal(request.Name, deck.Name);
			Assert.Equal(request.Description, deck.Description);
			Assert.Equal(request.PrimaryLanguage, deck.PrimaryLanguage);
			Assert.Equal(request.SecondaryLanguage, deck.SecondaryLanguage);
			Assert.Equal(request.CategoryId, deck.CategoryId);
		}

		[Fact]
		public async Task UpdateDeck_WhenDeckExists_SavesChanges()
		{
			// Arrange
			Deck deck = new Deck
			{
				Id = 56,
				Name = "Test deck",
				Description = "",
				PrimaryLanguage = "French",
				SecondaryLanguage = "Italian",
				AuthorId = 34,
				IsVisible = true,
				CategoryId = 12,
				IconPath = ""
			};
			DeckUpdateRequest request = new DeckUpdateRequest
			{
				Name = "Other test deck",
				Description = "Somedesc",
				PrimaryLanguage = "Italian",
				SecondaryLanguage = "French",
				CategoryId = 22
			};

			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(new List<Deck> { deck });
			_contextMock
				.Setup(context => context.Decks.FindAsync(deck.Id))
				.ReturnsAsync(deck);

			// Act
			await _service.UpdateDeck(56, request);

			// Assert
			_contextMock.Verify(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task UpdateDeck_WhenDeckDoesntExist_ThrowsNotFoundException()
		{
			// Arrange
			Deck deck = new Deck
			{
				Id = 56,
				Name = "Test deck",
				Description = "",
				PrimaryLanguage = "French",
				SecondaryLanguage = "Italian",
				AuthorId = 34,
				IsVisible = true,
				CategoryId = 12,
				IconPath = ""
			};
			DeckUpdateRequest request = new DeckUpdateRequest
			{
				Name = "Other test deck",
				Description = "Somedesc",
				PrimaryLanguage = "Italian",
				SecondaryLanguage = "French",
				CategoryId = 22
			};

			_contextMock
				.Setup(context => context.Decks)
				.ReturnsDbSet(new List<Deck> { deck });
			_contextMock
				.Setup(context => context.Decks.FindAsync(deck.Id))
				.ReturnsAsync(deck);

			// Act, Assert
			await Assert.ThrowsAsync<NotFoundException>(async () => await _service.UpdateDeck(65895, request));
		}
		#endregion
	}
}