using AutoMapper;
using LinguaDecks.Api.Models;
using LinguaDecks.Api.Models.DeckModels;
using LinguaDecks.Business.DTOs;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Models;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinguaDecks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	
	public class DecksController : ControllerBase
	{
		private readonly IDecksService _deckService;
		private readonly IMapper _mapper;
		private readonly IDeckSearcher _deckSearcher;
		private readonly IProgressService _progressService;
		public DecksController(IDecksService deckService, IProgressService progressService, IMapper mapper, IDeckSearcher deckSearcher)
		{
			_deckService = deckService;
			_mapper = mapper;
			_deckSearcher = deckSearcher;
			_progressService = progressService;
		}

		[HttpGet]
		[Route("{deckId}")]
		public async Task<IActionResult> GetDeck(int deckId)
		{
			DeckModel deckModel = _mapper.Map<DeckModel>(await _deckService.GetDeckById(deckId));

			var requiredComments = await _deckService.GetDecksComments(deckId);
			var comments = requiredComments.Select(_mapper.Map<CommentModel>);

			var deckRatings = await _deckService.GetDecksRatings(deckId);
			var calculatedRating = _deckService.CalculateDeckRating(deckRatings);
			DeckRatingModel ratings = new DeckRatingModel
			{
				Rating = calculatedRating.Rating,
				Votes = calculatedRating.Votes
			};

			DeckResponse response = new DeckResponse
			{
				Deck = deckModel,
				Comments = comments,
				DeckRating = ratings
			};

			return Ok(response);
		}
		[HttpGet]
		public async Task<IActionResult> GetDecks(int page, int pageSize, [FromQuery] SearchParameters searchParameters)
		{
			var result = await _deckSearcher.FindDeckAsync(page, pageSize, searchParameters);
			var targetDecks = result.Select(deck => _mapper.Map<DeckPreview>(deck)).ToList();

			DecksResponse response = new DecksResponse
			{
				DeckItems = targetDecks,
				TotalDecks = _deckSearcher.DecksCount,
			};

			return Ok(response);
		}

		[HttpGet]
		[Route("{deckId}/cards")]
		public async Task<IActionResult> GetCards(int deckId)
		{
			DeckModel deckModel = _mapper.Map<DeckModel>(await _deckService.GetDeckById(deckId));
			var response = new 
			{ 
				DeckId = deckModel.Id, 
				DeckName = deckModel.Name, 
				deckModel.Cards 
			};

			return Ok(response);
		}

		[HttpPost]
		[Route("{deckId}/progress")]
		public async Task<IActionResult> SaveProgress([FromBody] SaveProgressRequest progressRequest)
		{
			var cardProgresses = _mapper.Map<IEnumerable<CardProgress>>(progressRequest);

			await _deckService.SaveProgress(cardProgresses);

			return Ok();
		}

		[HttpGet]
		[Route("icons")]
		public async Task<IActionResult> GetDeckIcons([FromHeader] IEnumerable<int> deckIds)
		{
			var result = await _deckService.GetDeckIcons(deckIds);

			List<DeckIconResponse> response = new List<DeckIconResponse>();

			foreach (var item in result)
			{
				response.Add(new DeckIconResponse 
				{ 
					DeckId = item.deckId, 
					IconPath = item.iconPath 
				});
			}

			return Ok(response);
		}

		[HttpGet]
		[Route("{deckId}/progress")]
		[Authorize]
		public async Task<IActionResult> GetCalculatedProgress(int deckId)
		{
			var claim = HttpContext.User.FindFirst("userId");
			int userId = int.Parse(claim.Value);
			var progressCards = (await _progressService.GetDeckProgress(userId, deckId)).ToList();
			var calculatedProgress = await _progressService.CalculateProgress(deckId, progressCards);
			ProgressModel result = new()
			{
				Positive = calculatedProgress.positive,
				Negative = calculatedProgress.negative,
				Total = calculatedProgress.total
			};

			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> CreateDeck([FromForm] DeckCreateRequest request)
		{
			DeckModel deckModel = _mapper.Map<DeckModel>(await _deckService.CreateDeck(request));

			return CreatedAtRoute(new { deckId = deckModel.Id }, deckModel);
		}

		[HttpPut]
		[Route("{deckId}")]
		public async Task<IActionResult> UpdateDeck(int deckId, [FromForm] DeckUpdateRequest request)
		{
			await _deckService.UpdateDeck(deckId, request);

			return NoContent();
		}

		[HttpDelete]
		[Route("{deckId}")]
		public async Task<IActionResult> DeleteDeck(int deckId)
		{
			await _deckService.DeleteDeck(deckId);

			return NoContent();
		}
	}
}
