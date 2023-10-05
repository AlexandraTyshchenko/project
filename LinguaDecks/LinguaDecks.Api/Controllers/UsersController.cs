using AutoMapper;
using LinguaDecks.Api.Models;
using LinguaDecks.Business.DTOs;
using LinguaDecks.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinguaDecks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public UsersController(IUserService userService, IMapper mapper)
		{
			_userService = userService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers(int page, int pageSize, [FromQuery] SearchUsersParameters searchParameters)
		{
			var result = await _userService.FindUsers(page, pageSize, searchParameters);
			var usersResponse = result.users.Select(user => _mapper.Map<SimpleUserModel>(user));

			var response = new UsersResponse
			{
				TotalUsers = result.usersCount,
				UserItems = usersResponse
			};
			return Ok(response);
		}

		[HttpGet]
		[Route("{userId}")]
		public async Task<IActionResult> GetUserData(int userId, int page, int pageSize)
		{
			var userModel = await _userService.GetUser(userId);
			var userResponse = _mapper.Map<SimpleUserModel>(userModel);

			return Ok(userResponse);
		}
	}
}
