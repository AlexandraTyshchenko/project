using AutoMapper;
using LinguaDecks.Api.Models;
using LinguaDecks.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinguaDecks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ProfileController : Controller
	{
		private readonly IDecksService _deckService;
		private readonly IProgressService _progressService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public ProfileController(IDecksService deckService, IProgressService cardsService, IUserService userService, IMapper mapper)
		{
			_deckService = deckService;
			_progressService = cardsService;
			_userService = userService;
			_mapper = mapper;
		}

		

		[HttpGet]
		[Route("statistics")]
		public async Task<IActionResult> GetCalculatedTotalProgress()
		{
			var user = HttpContext.User.FindFirst("userId");
			int userId = int.Parse(user.Value);
			var totalProgress = await _progressService.GetTotalProgress(userId);
			ProgressModel result = new()
			{
				Positive = 0,
				Negative = 0,
				Total = 0
			};
			foreach (var item in totalProgress)
			{
				var res = await _progressService.CalculateProgress(item.Key, item.Value);

				result.Positive += res.positive;
				result.Negative += res.negative;
				result.Total += res.total;
			}

			return Ok(result);
		}

		[HttpGet]
		[Route("progress")]
		public async Task<IActionResult> GetTotalProgress()
		{
			var user = HttpContext.User.FindFirst("userId");
			int userId = int.Parse(user.Value);

			var totalProgress = await _progressService.GetTotalProgress(userId);
			List<DeckProgressModel> response = new();

			foreach (var item in totalProgress)
			{
				var calculatedProgress = await _progressService.CalculateProgress(item.Key, item.Value);
				var calculatedProgressModel = _mapper.Map<ProgressModel>(calculatedProgress);
				response.Add(new DeckProgressModel
				{
					DeckId = item.Key,
					DeckName = (await _deckService.GetDeckById(item.Key)).Name,
					Progress = calculatedProgressModel
				});
			}

			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetUserData()
		{
			var user = HttpContext.User.FindFirst("userId");
			int userId = int.Parse(user.Value);
			var userModel = await _userService.GetUser(userId);
			var userResponse = _mapper.Map<SimpleUserModel>(userModel);

			return Ok(userResponse);
		}
	}
}
