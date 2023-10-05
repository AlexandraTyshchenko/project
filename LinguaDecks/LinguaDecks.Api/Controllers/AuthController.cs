using AutoMapper;
using LinguaDecks.Api.Models.AuthModels;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Models;
using LinguaDecks.DataAccess.Entities;
using LinguaDecks.DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinguaDecks.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthenticationService _authenticationService;

		private readonly ITokenService _tokenService;

		private readonly ITeacherRequestCreator _requestCreatorService;

		private readonly IMapper _mapper;

		public AuthController(IAuthenticationService authenticationService, ITokenService tokenService, ITeacherRequestCreator requestCreatorService, IMapper mapper)
		{
			_authenticationService = authenticationService;
			_tokenService = tokenService;
			_requestCreatorService = requestCreatorService;
			_mapper = mapper;
		}

		[HttpPost]
		[Route("signin")]
		public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
		{
			User user = await _authenticationService.SignInAsync(request);
			TokenResponseModel response = _mapper.Map<TokenResponseModel>(await _tokenService.CreateTokenAsync(user));

			return Ok(response);
		}

		[HttpPost]
		[Route("signup")]
		public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
		{
			User user = await _authenticationService.SignUpAsync(request);

			if (request.Role == Roles.Teacher)
			{
				await _requestCreatorService.CreateAsync(user.Id, request.Description);
			}

			return Ok();
		}

		[HttpPost]
		[Route("refresh")]
		public async Task<IActionResult> Refresh([FromBody] string token)
		{
			TokenResponseModel response = _mapper.Map<TokenResponseModel>(await _tokenService.RefreshTokenAsync(token));

			return Ok(response);
		}
	}
}
