using AutoMapper;
using LinguaDecks.Api.Models;
using LinguaDecks.Business.Exceptions;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LinguaDecks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TeacherRequestsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IRequestsProvider _requestsProvider;
		private readonly IRequestManager _requestManager;

		public TeacherRequestsController(IMapper mapper, IRequestsProvider requestsProvider, IRequestManager requestManager)
		{
			_mapper = mapper;
			_requestsProvider = requestsProvider;
			_requestManager = requestManager;
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> Get(int page, int pageSize, Statuses status)
		{
			var requests = await _requestsProvider.GetTeacherRequests(page, pageSize, status);
			var result = requests.Select(request => _mapper.Map<TeacherRequestModel>(request)).ToList();
			TeacherRequestsResponse response = new TeacherRequestsResponse
			{
				TeacherRequests = result,
				TotalTeacherRequests = _requestsProvider.TotalRequestsCount
			};
			return Ok(response);
		}


		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> AnswerRequest(int id, bool answer)
		{
				
			await _requestManager.AnswerRequest(id, answer);
		
			return Ok();

		}

	}
}
