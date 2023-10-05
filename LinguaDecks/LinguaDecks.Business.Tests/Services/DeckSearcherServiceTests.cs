using LinguaDecks.Business.DTOs;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using LinguaDecks.DataAccess.Entities;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace LinguaDecks.Business.Tests.Services
{
	[Trait("Category", "Unit")]
	public class DeckSearcherServiceTests
	{
		private readonly Mock<ApplicationContext> _contextMock;
		private readonly IDeckSearcher _deckSearcher;
		private readonly List<Deck> _decks;

		public DeckSearcherServiceTests()
		{
			_contextMock = new Mock<ApplicationContext>();
			_deckSearcher = new DeckSearcher(_contextMock.Object);
			_decks = new List<Deck>
			{
				new Deck
				{
					Description = "some description1",
					Name = "some name1",
					AuthorId=1,
					Author = new User()
					{
						Id=1,
						Name="some author name1"
					},
					Id = 1,
					PrimaryLanguage = "English",
					SecondaryLanguage = "French",
					CategoryId = 1,
				},
				new Deck
				{
					Description = "some description2",
					Name = "some name2",
					Id = 2,
					Author = new User()
					{
						Id=2,
						Name="some author name2"
					},
					AuthorId=2,
					PrimaryLanguage = "French",
					SecondaryLanguage = "English",
					CategoryId = 2,
				},
				new Deck
				{
					Description = "some description3",
					Name = "some name3",
					Author = new User()
					{
						Id=3,
						Name="some author name3"
					},
					Id = 3,
					AuthorId=3,
					PrimaryLanguage = "English",
					SecondaryLanguage = "French",
					CategoryId = 3,
				}
			};
		}

		[Fact]
		public void FindDeckAsync_EmptyParameters_ReturnsDecks()
		{
			// Arrange
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);

			// Act
			var result = _deckSearcher.FindDeckAsync(1, 10, new SearchParameters()).Result;

			// Assert
			Assert.Equal(3, result.Count);
		}

		[Fact]
		public void FindDeckAsync_NameParameter_ReturnsDecksWithNameParameter()
		{
			// Arrange
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);

			// Act
			var result = _deckSearcher.FindDeckAsync(1, 10, new SearchParameters { Name = "some name2" }).Result;

			// Assert
			Assert.Single(result);
			Assert.All(result, deck => Assert.Equal("some name2", deck.Name));
		}

		[Fact]
		public void FindDeckAsync_PrimaryLanguageAndSecondaryLanguageParameter_ReturnsDecksWithLanguagesParameters()
		{
			// Arrange
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);

			// Act
			var result = _deckSearcher.FindDeckAsync(1, 10, new SearchParameters { PrimaryLanguage = "French", SecondaryLanguage = "English" }).Result;

			// Assert
			Assert.Single(result);
			Assert.All(result, deck => Assert.Equal("French", deck.PrimaryLanguage));
			Assert.All(result, deck => Assert.Equal("English", deck.SecondaryLanguage));
		}

		[Fact]
		public void FindDeckAsync_CategoryIdParameter_ReturnsDecksWithCategoryIdParameter()
		{
			// Arrange
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);

			// Act
			var result = _deckSearcher.FindDeckAsync(1, 10, new SearchParameters { CategoryId = 1 }).Result;

			// Assert
			Assert.Single(result);
			Assert.All(result, deck => Assert.Equal(1, deck.CategoryId));
		}

		[Fact]
		public void FindDeckAsync_AuthorParameter_ReturnsDecksWithAuthorParameter()
		{
			// Arrange
			string AuthorName = "some author name1";
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);
			// Act
			var result = _deckSearcher.FindDeckAsync(1, 10, new SearchParameters { Author = AuthorName }).Result;

			// Assert
			Assert.Single(result);
			Assert.All(result, deck => Assert.Equal(1, deck.AuthorId));
		}

		[Fact]
		public void FindDeckAsync_UnmatchingParameters_ReturnsEmptyListOfDecks()
		{
			// Arrange
			string AuthorName = "some unexisting name";
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);
			// Act
			var result = _deckSearcher.FindDeckAsync(1, 10, new SearchParameters { Author = AuthorName }).Result;

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void FindDeckAsync_WithAllMatchingParameters_ReturnsListOfDecks()
		{
			// Arrange
			var SearchParameters = new SearchParameters
			{
				Name = "some name2",
				Author = "some author name2",
				PrimaryLanguage = "French",
				SecondaryLanguage = "English",
				CategoryId = 2
			};
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);
			// Act
			var result = _deckSearcher.FindDeckAsync(1, 10, SearchParameters).Result;

			// Assert
			Assert.Single(result);
			Assert.All(result, deck => Assert.Equal(2, deck.AuthorId));
			Assert.All(result, deck => Assert.Equal(SearchParameters.Name, deck.Name));
			Assert.All(result, deck => Assert.Equal(SearchParameters.PrimaryLanguage, deck.PrimaryLanguage));
			Assert.All(result, deck => Assert.Equal(SearchParameters.SecondaryLanguage, deck.SecondaryLanguage));
			Assert.All(result, deck => Assert.Equal(SearchParameters.CategoryId, deck.CategoryId));
		}

		[Fact]
		public void FindDeckAsync_WithOneUnMatchingParameter_ReturnsEmptyList()
		{
			// Arrange
			var SearchParameters = new SearchParameters
			{
				Name = "some name2",
				Author = "some unexisting name2",
				PrimaryLanguage = "French",
				SecondaryLanguage = "English",
				CategoryId = 2
			};
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);
			// Act
			var result = _deckSearcher.FindDeckAsync(1, 10, SearchParameters).Result;

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void FindDeckAsync_WithNullParameters_ReturnsListOfAllDecks()
		{
			// Arrange
			var SearchParameters = new SearchParameters
			{
				Name = null,
				Author = null,
				PrimaryLanguage = null,
				SecondaryLanguage = null,
				CategoryId = 0
			};
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);
			// Act
			var result = _deckSearcher.FindDeckAsync(1, 10, SearchParameters).Result;

			// Assert
			Assert.Equal(3, result.Count);
		}

		[Fact]
		public void FindDeckAsync_UnexistingPage_ReturnsEmptyList()
		{
			// Arrange
			var SearchParameters = new SearchParameters
			{
				Name = null,
				Author = null,
				PrimaryLanguage = null,
				SecondaryLanguage = null,
				CategoryId = 0
			};
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);
			// Act
			var result = _deckSearcher.FindDeckAsync(2, 10, SearchParameters).Result;

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void FindDeckAsync_PageSizeIsLessThanNumberOfPageResults_ReturnsDevidedResults()
		{
			// Arrange
			var SearchParameters = new SearchParameters
			{
				Name = null,
				Author = null,
				PrimaryLanguage = null,
				SecondaryLanguage = null,
				CategoryId = 0
			};
			_contextMock.Setup(context => context.Decks)
				.ReturnsDbSet(_decks);
			// Act
			var result = _deckSearcher.FindDeckAsync(1, 2, SearchParameters).Result;

			// Assert
			Assert.Equal(2, result.Count);
		}
	}
}
