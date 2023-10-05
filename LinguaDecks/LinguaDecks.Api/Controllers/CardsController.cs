using AutoMapper;
using LinguaDecks.Api.Models;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinguaDecks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CardsController : Controller
	{
		private readonly ICardsService _cardsService;
		private readonly IMapper _mapper;

		public CardsController(ICardsService cardsService, IMapper mapper)
		{
			_cardsService = cardsService;
			_mapper = mapper;
		}

		[HttpPut]
		public async Task<IActionResult> AddCard([FromBody] AddCardRequest addRequest)
		{
			Card card = _mapper.Map<Card>(addRequest);
			await _cardsService.AddCard(card);
			return Ok();
		}

		[HttpDelete]
		[Route("{cardId}")]
		public async Task<IActionResult> DeleteCard(int cardId)
		{
			await _cardsService.DeleteCard(cardId);
			return Ok();
		}
	}
}
